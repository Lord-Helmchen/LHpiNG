using ScrapySharp.Network;
using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using LHpiNG.Cardmarket;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;
using ScrapySharp.Exceptions;
using System.Diagnostics;
using System.Threading;
using ScrapySharp.Html.Forms;

namespace LHpiNG.Web
{
    internal sealed class Scraper : IFromCardmarket
    {
        private ScrapingBrowser Browser { get; set; }
        private string UrlServerPrefix { get; set; }
        private int DelayMiliseconds { get; set; }
        private string UrlResultsPerPageSuffix { get; set; }

        private static readonly Lazy<Scraper> lazy = new Lazy<Scraper>(() => new Scraper());

        /// <summary>get singleton instance of Scraper</summary>
        public static Scraper Instance { get { return lazy.Value; } }

        private Scraper()
        {
            Browser = new ScrapingBrowser
            {
                // Browser has settings you can access in setup
                AllowAutoRedirect = true,
                AllowMetaRedirect = true,
                Language = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-DE")
                //,UserAgent = // TODO include "This wouldn't be necessary if you hadn't revoked my API access" in AgentString
            };
            UrlServerPrefix = "https://www.cardmarket.com";
            UrlResultsPerPageSuffix = "?perSite=50";
            DelayMiliseconds = 100;
        }

        private WebPage FetchPage(Uri uri, int delay = 0)
        {
            try
            {
                WebPage result;
                bool fetchedPage = false;
                do
                {
                    DelayRequest(delay);
                    fetchedPage = TryFetchPage(uri, out result);
                }
                while (!fetchedPage);
                return result;
            }
            catch (Exception ex) when (ex is HttpException || ex is WebException)
            {
                Console.WriteLine(ex.Message);
#if DEBUG
                throw;
#endif
#pragma warning disable CS0162 // Unreachable code detected
                return null;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        private bool TryFetchPage(Uri uri, out WebPage page)
        {
            try
            {
                page = Browser.NavigateToPage(uri);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Single() is WebException)
                {
                    throw ex.InnerException;
                }
                else { throw; }
            }

            if ((int)HttpStatusCode.OK == page?.RawResponse.StatusCode && page.RawResponse.Body.Length > 0)
            {
                string msg = String.Format("got {0} with status {1}", uri.AbsoluteUri, page.RawResponse.StatusCode);
                int msgSpaces = Console.WindowWidth - msg.Length - 3;
                msgSpaces = (msgSpaces > 0) ? msgSpaces : 0;
                Console.Write(String.Concat("\r", msg, new String(' ', msgSpaces)));
                DelayMiliseconds = DelayMiliseconds == 1 ? 1 : DelayMiliseconds / 2;
                return true;
            }
            else if ((int)HttpStatusCode.OK == page.RawResponse.StatusCode)
            {
                DelayMiliseconds = DelayMiliseconds * 2;
                return false;
            }
            else
            {
                string message = String.Format("unhandled response status: {0} ({1})", page.RawResponse.StatusCode, page.RawResponse.StatusDescription);
                throw new HttpException(page.RawResponse.StatusCode, page.RawResponse.StatusDescription);
            }
        }

        private void DelayRequest(int delay = 0)
        {
            DelayMiliseconds = delay > 0 ? delay : DelayMiliseconds;
            //wait for flood protection to calm down
            if (DelayMiliseconds > 0)
            {
                if (DelayMiliseconds > 999)
                {
                    Console.Write($"\rWaiting {DelayMiliseconds / 1000} seconds for flood protection to calm down");
                }
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (true)
                {
                    //some other processing to do STILL POSSIBLE here
                    if (stopwatch.ElapsedMilliseconds >= DelayMiliseconds)
                    {
                        break;
                    }
                    Thread.Sleep(1); //so processor can rest for a while
                }
            }
        }

        //TODO unroll SubmitForm recursion, reuse code from FetchPage as much as possible
        private WebPage SubmitForm(PageWebForm form, int delay = 0)
        {
            DelayRequest();
            try
            {
                WebPage result = form.Submit();

                if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode && result.RawResponse.Body.Length > 0)
                {
                    string msg = String.Format("got reply to form {0} with status {1}", form.Action, result.RawResponse.StatusCode);
                    int msgSpaces = Console.WindowWidth - msg.Length - 3;
                    msgSpaces = (msgSpaces > 0) ? msgSpaces : 0;
                    Console.Write(String.Concat("\r", msg, new String(' ', msgSpaces)));
                    DelayMiliseconds = DelayMiliseconds == 1 ? 1 : DelayMiliseconds / 2;
                    return result;
                }
                else if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode)
                {
                    //recursively work around flood protection
                    DelayMiliseconds = DelayMiliseconds * 2;
                    return SubmitForm(form, DelayMiliseconds);
                }
                else
                {
                    string message = String.Format("unhandled response status: {0} ({1})", result.RawResponse.StatusCode, result.RawResponse.StatusDescription);
                    Console.WriteLine(message);
                    throw new HttpException(result.RawResponse.StatusCode, result.RawResponse.StatusDescription);
                }

            }
            catch (HttpException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        #region expansions

        /// <summary>
        /// implements interface IFromCardmarket
        /// </summary>
        public ExpansionList ImportExpansionList()
        {
            ExpansionList expansionList = new ExpansionList();

            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlServerPrefix, expansionList.UrlSuffix)));
            IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("div.expansion-row");

            foreach (HtmlNode node in nodes)
            {
                Expansion expansion = ParseExpansion(node);
                expansionList.Add(expansion);
            }
            expansionList.FetchedOn = DateTime.Now;

            return expansionList;
        }

        private Expansion ParseExpansion(HtmlNode node)
        {
            try
            {
                Expansion expansion = new Expansion
                {
                    EnName = node.ChildNodes[2].ChildNodes[0].InnerText,
                    ReleaseDate = node.ChildNodes[4].InnerText,
                    UrlSuffix = node.ChildNodes[2].ChildNodes[0].GetAttributeValue("href")
                };
                var productCount = Regex.Match(node.ChildNodes[3].InnerText, @"^(\d+) Cards").Groups[1].Value;
                expansion.ProductCount = int.Parse(productCount);
                if (DateTime.TryParse(expansion.ReleaseDate, out DateTime parsedDate))
                {
                    expansion.ReleaseDateTime = parsedDate;
                }
                else if (DateTime.TryParse(Regex.Replace(expansion.ReleaseDate, @"^(\d+)[snrt][tdh]", "$1"), out parsedDate))//remove "st" from "1st" etc
                {
                    expansion.ReleaseDateTime = parsedDate;
                }
                else
                {
                    throw new FormatException("Failed to parse Release Date!");
                }
                expansion.IsReleased = parsedDate.Date < DateTime.Now.Date;
                expansion.ProductsUrlSuffix = ScrapeProductsUrlSuffix(expansion); ;

                return expansion;
            }
            catch// (Exception ex)
            {
                throw;
            }
        }

        private string ScrapeProductsUrlSuffix(Expansion expansion, bool doGuess = true)
        {
            //shortcut nonexisting Singles url
            if (expansion.ProductCount <= 0)
            {
                return null;
            }

            if (doGuess)
            {
                return Regex.Replace(expansion.UrlSuffix, @"^.*[^/]/", "/en/Magic/Products/Singles/");
            }
            else
            {
                WebPage resultpage = FetchPage(new Uri(String.Concat(UrlServerPrefix, expansion.UrlSuffix, UrlResultsPerPageSuffix)));
                IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("a.card");
                string productsUrlSuffix = null;
                foreach (HtmlNode node in nodes)
                {
                    if (node.GetAttributeValue("href").Contains("Singles"))
                    {
                        productsUrlSuffix = node.GetAttributeValue("href");
                        break;
                    }
                }
                return productsUrlSuffix;
            }
        }

        #endregion

        #region products

        /// <summary>
        /// implements interface IFromCardmarket
        /// </summary>
        public ICollection<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            if (!(expansion is Expansion))
            {
                throw new ArgumentException("Need Expansion instead of only ExpansionEntity");
            }

            List<ProductEntity> products = new List<ProductEntity>();
            string urlSuffix = String.Concat(((Expansion)expansion).ProductsUrlSuffix, UrlResultsPerPageSuffix);

            while (urlSuffix != String.Empty)
            {
                string productsUrl = String.Concat(UrlServerPrefix, urlSuffix);
                urlSuffix = ParseProducts(expansion, products, productsUrl);
            }

            if (products.Count() != ((Expansion)expansion).ProductCount)
            {
                string msg = $"Product Count mismatch (found {products.Count()}, expected {((Expansion)expansion).ProductCount})";
                Console.WriteLine(msg);
                throw new ScrapingException(msg);
            }
            if (products.All(p => p.Number.Length > 0))
            {
                return products.OrderBy(x => x.Number).ToList();
            }
            else
            {
                return products.OrderBy(x => x.EnName).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expansion">the Products' expansion</param>
        /// <param name="products">parsed Products are added here</param>
        /// <param name="productsUrl">url without </param>
        /// <returns>next pagination url or null when last</returns>
        private String ParseProducts(ExpansionEntity expansion, List<ProductEntity> products, string url)
        {
            WebPage resultpage = FetchPage(new Uri(url));
            if (resultpage == null) return String.Empty;

            HtmlNode table = resultpage?.Html?.CssSelect("div.table-body")?.First();
            foreach (HtmlNode row in table?.ChildNodes ?? Enumerable.Empty<HtmlNode>())
            {
                ProductEntity product = new Product
                {
                    EnName = row.ChildNodes[3].FirstChild.FirstChild.FirstChild.InnerText,
                    ExpansionName = expansion.EnName,
                    Number = row.ChildNodes[3].FirstChild.ChildNodes[1].InnerText,
                    Website = row.ChildNodes[3].FirstChild.FirstChild.FirstChild.GetAttributeValue("href"),
                    Expansion = expansion
                };
                if (int.TryParse(product.Number, out int parsedNumber))
                {
                    ((Product)product).CollNr = parsedNumber;
                }

                string rarityString = row.ChildNodes[3].FirstChild.ChildNodes[2].FirstChild.GetAttributeValue("data-original-title");
                if (Enum.TryParse(rarityString, true, out Rarity parsedRarity))
                {
                    product.Rarity = parsedRarity;
                }
                //TODO can I scrape reprintCount ? -> yes, but only from priceGuide page
                products.Add(product);
            }
            HtmlNode pagination = resultpage?.Html?.CssSelect("div#pagination")?.First();
            string nextUrlSuffix = pagination.CssSelect("a.btn").Last().GetAttributeValue("href") ?? String.Empty;

            return nextUrlSuffix;
        }

        #endregion

        #region prices

        [Obsolete]
        public IEnumerable<ProductEntity> ImportPriceGuides(IEnumerable<ProductEntity> products)
        {
            foreach (Product product in products)
            {
                product.PriceGuide = ImportPriceGuide(product);
                //would have to add to price history (or probably do it in called method)
            }
            return products;
        }

        /// <summary>
        /// implements interface IFromCardmarket
        /// </summary>
        public IList<PriceGuide> ImportPriceGuides(ProductEntity product)
        {
            //TODO recreate previous priceGuidePros from graph
            //probably want to split common parts of ImportPriceGuide for this
            throw new NotImplementedException();
        }

        /// <summary>
        /// implements interface IFromCardmarket
        /// </summary>
        public PriceGuideEntity ImportPriceGuide(ProductEntity product)
        {
            PriceGuide priceGuide = new PriceGuide()
            {
                Product = product as Product,
                IdProduct = product.IdProduct
            };

            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlServerPrefix, product.Website)));
            HtmlNode infoListNode = resultpage.Html.CssSelect(".info-list-container").First().FirstChild;

            if (int.TryParse(infoListNode.ChildNodes[9].InnerText, out int parsedAvailable) && parsedAvailable > 0)
            {
                string LowexPlus = Regex.Replace(infoListNode.ChildNodes[11].InnerText, @"^(\d+)[.,](\d+).*", "$1$2");
                if (decimal.TryParse(LowexPlus, out decimal LowexPlusParsed))
                {
                    priceGuide.LowexPlus = LowexPlusParsed / 100;
                }
                else
                {
                    throw new ScrapingException("Could not parse LowexPlus");
                }
                //scraping trend from graph works even when 0 available, so we'll prefer that.
                //string trend = Regex.Replace(infoListNode.ChildNodes[13].FirstChild.InnerText, @"^(\d+)[.,](\d+).*", "$1$2");
                //if (decimal.TryParse(trend, out decimal trendParsed))
                //{
                //    guideEntity.Trend = trendParsed / 100;
                //}
                //else
                //{
                //    throw new ScrapingException("Could not parse LowexPlus");
                //}
            }
            string javaScriptString = Regex.Replace(resultpage.Html.CssSelect(".chart-wrapper").First().ChildNodes[1].InnerText, "\\\"", "");
            string[] sellArray = Regex.Match(javaScriptString, @"Avg\. Sell Price,data:\[([^\]]+)]").Groups[1].Value.Split(',');
            if (decimal.TryParse(sellArray.Last(), out decimal parsedSell))
            {
                priceGuide.Sell = parsedSell;
            }
            else
            {
                throw new ScrapingException("Could not parse Sell from graph");
            }
            string[] trendArray = Regex.Match(javaScriptString, @"Price Trend,data:\[([^\]]+)]").Groups[1].Value.Split(',');
            if (decimal.TryParse(trendArray.Last(), out decimal parsedTrend))
            {
                priceGuide.Trend = parsedTrend;
            }
            else
            {
                throw new ScrapingException("Could not parse Trend from graph");
            }
            string[] dateArray = Regex.Match(javaScriptString, @"labels:\[([^\]]+)]").Groups[1].Value.Split(',');
            if (DateTime.TryParse(dateArray.Last(), out DateTime parsedDate))
            {
                priceGuide.FetchDate = parsedDate;
            }
            else
            {
                priceGuide.FetchDate = DateTime.Now;
                throw new ScrapingException("Could not parse Date from graph");
            }

            PageWebForm form = resultpage.FindFormById("FilterForm");
            form["extra[isFoil]"] = "Y";
            WebPage foilResultPagex = SubmitForm(form);
            Browser.ClearCookies(); // reset session, so the (foil-)filter will be reset

            javaScriptString = Regex.Replace(foilResultPagex.Html.CssSelect(".chart-wrapper").First().ChildNodes[1].InnerText, "\\\"", "_");
            string[] foilSellArray = Regex.Match(javaScriptString, @"Avg\. Sell Price_,_data_:\[([^\]]+)]").Groups[1].Value.Split(',');
            if (decimal.TryParse(sellArray.Last(), out decimal parsedFoilSell))
            {
                priceGuide.FoilSell = parsedFoilSell;
            }
            else
            {
                throw new ScrapingException("Could not parse FoilSell from graph");
            }
            string[] foilTrendArray = Regex.Match(javaScriptString, @"Price Trend_,_data_:\[([^\]]+)]").Groups[1].Value.Split(',');
            if (decimal.TryParse(foilTrendArray.Last(), out decimal parsedFoilTrend))
            {
                priceGuide.FoilTrend = parsedFoilTrend;
            }
            else
            {
                throw new ScrapingException("Could not parse FoilTrend from graph");
            }

            return priceGuide;
        }
        #endregion




    }
}


