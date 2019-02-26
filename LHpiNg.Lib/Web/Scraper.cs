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
            string targetUrl = uri.AbsoluteUri;
            try
            {
                return CheckResult(page, targetUrl);
            }
            catch (HttpException) { throw; }
        }

        private WebPage SubmitForm(PageWebForm form, int delay = 0)
        {
            try
            {
                WebPage result;
                bool fetchedPage = false;
                do
                {
                    DelayRequest(delay);
                    fetchedPage = TrySubmitForm(form, out result);
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

        private bool TrySubmitForm(PageWebForm form, out WebPage page)
        {
            try
            {
                page = form.Submit();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Single() is WebException)
                {
                    throw ex.InnerException;
                }
                else { throw; }
            }
            string targetUrl = form.Action;
            try
            {
                return CheckResult(page, targetUrl);
            }
            catch (HttpException) { throw; }
        }

        private bool CheckResult(WebPage page, string targetUrl)
        {
            if ((int)HttpStatusCode.OK == page?.RawResponse.StatusCode && page.RawResponse.Body.Length > 0)
            {
                string msg = String.Format("got {0} with status {1}", targetUrl, page.RawResponse.StatusCode);
                int msgSpaces = Console.WindowWidth - msg.Length - 1;
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

        //wait for flood protection to calm down
        private void DelayRequest(int delay = 0)
        {
            DelayMiliseconds = delay > 0 ? delay : DelayMiliseconds;
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
                    Thread.Sleep(1);
                }
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
            expansionList.FetchedOn = DateTime.Today;

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
                expansion.IsReleased = parsedDate.Date < DateTime.Today;
                expansion.ProductsUrlSuffix = ScrapeProductsUrlSuffix(expansion);
                expansion.Uid = Sha256Helper.GenerateHashBytes(expansion.EnName);

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
            string urlSuffix = ((Expansion)expansion).ProductsUrlSuffix ?? String.Empty;
            urlSuffix = urlSuffix == String.Empty ? String.Empty : String.Concat(urlSuffix, UrlResultsPerPageSuffix);

            while (urlSuffix != String.Empty)
            {
                string productsUrl = String.Concat(UrlServerPrefix, urlSuffix);
                urlSuffix = ParseProducts(expansion, productsUrl, out List<ProductEntity> parsedProducts);
                products.AddRange(parsedProducts);
            }

            if (products.Count() != ((Expansion)expansion).ProductCount)
            {
                string msg = $"Product Count mismatch (found {products.Count()}, expected {((Expansion)expansion).ProductCount})";
                Console.WriteLine(msg);
                throw new ScrapingException(msg);
            }
            if (products.All(p => p.Number.Length > 0))
            {
                return products.OrderBy(p => ((Product)p).CollNr ?? 0).ThenBy(p => p.Number).ToList();
            }
            else
            {
                return products.OrderBy(p => p.EnName).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expansion">the Products' expansion</param>
        /// <param name="url"></param>
        /// <param name="products"></param>
        /// <returns>next pagination url or null when last</returns>        
        private String ParseProducts(ExpansionEntity expansion, string url, out List<ProductEntity> products)
        {
            WebPage resultpage = FetchPage(new Uri(url));
            products = new List<ProductEntity>();
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

                string rarityString = row.ChildNodes[3].FirstChild.ChildNodes[2].FirstChild.FirstChild.GetAttributeValue("data-original-title");
                rarityString = Regex.Replace(rarityString, " ", "");
                if (Enum.TryParse(rarityString, true, out Rarity parsedRarity))
                {
                    product.Rarity = parsedRarity;
                }
                else
                {
                    product.Rarity = Rarity.None;
                }
                //TODO can I scrape reprintCount ? -> yes, but only from priceGuide page
                ((Product)product).Uid = Sha256Helper.GenerateHashBytes(String.Concat(product.EnName, product.ExpansionName));
                products.Add(product);
            }
            HtmlNode pagination = resultpage?.Html?.CssSelect("div#pagination")?.First();
            string nextUrlSuffix = pagination.CssSelect("a.btn").Last().GetAttributeValue("href") ?? String.Empty;

            return nextUrlSuffix;
        }

        #endregion

        #region prices

        /// <summary>
        /// implements interface IFromCardmarket
        /// </summary>
        public IEnumerable<PriceGuide> ImportPriceGuides(ProductEntity product)
        {
            IList<PriceGuide> priceGuides = new List<PriceGuide>();

            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlServerPrefix, product.Website)));
            HtmlNode infoListNode = resultpage.Html.CssSelect(".info-list-container").First().FirstChild;

            PriceGuide priceGuide = new PriceGuide
            {
                FetchDate = DateTime.Today,
            };

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
                string trend = Regex.Replace(infoListNode.ChildNodes[13].FirstChild.InnerText, @"^(\d+)[.,](\d+).*", "$1$2");
                if (decimal.TryParse(trend, out decimal trendParsed))
                {
                    priceGuide.Trend = trendParsed / 100;
                }
                else
                {
                    throw new ScrapingException("Could not parse Trend");
                }
            }

            PageWebForm form = resultpage.FindFormById("FilterForm");
            form["extra[isFoil]"] = "Y";
            WebPage foilResultPage = SubmitForm(form);
            Browser.ClearCookies(); // reset session, so the (foil-)filter will be reset
            HtmlNode foilInfoListNode = foilResultPage.Html.CssSelect(".info-list-container").First().FirstChild;
            if (int.TryParse(foilInfoListNode.ChildNodes[9].InnerText, out int parsedFoilAvailable) && parsedFoilAvailable > 0)
            {
                string lowFoil = Regex.Replace(foilInfoListNode.ChildNodes[11].InnerText, @"^(\d+)[.,](\d+).*", "$1$2");
                if (decimal.TryParse(lowFoil, out decimal lowFoilParsed))
                {
                    priceGuide.LowFoil = lowFoilParsed / 100;
                }
                else
                {
                    throw new ScrapingException("Could not parse LowexPlus");
                }
                string foilTrend = Regex.Replace(foilInfoListNode.ChildNodes[13].FirstChild.InnerText, @"^(\d+)[.,](\d+).*", "$1$2");
                if (decimal.TryParse(foilTrend, out decimal foilTrendParsed))
                {
                    priceGuide.FoilTrend = foilTrendParsed / 100;
                }
                else
                {
                    throw new ScrapingException("Could not parse FoilTrend");
                }
            }
            priceGuides.Add(priceGuide);

            string javaScriptString = Regex.Replace(resultpage.Html.CssSelect(".chart-wrapper").First().ChildNodes[1].InnerText, "\\\"", "");
            string[] dateArray = Regex.Match(javaScriptString, @"labels:\[([^\]]+)]").Groups[1].Value.Split(',');
            string[] sellArray = Regex.Match(javaScriptString, @"Avg\. Sell Price,data:\[([^\]]+)]").Groups[1].Value.Split(',');
            string[] trendArray = Regex.Match(javaScriptString, @"Price Trend,data:\[([^\]]+)]").Groups[1].Value.Split(',');
            string foilJavaScriptString = Regex.Replace(foilResultPage.Html.CssSelect(".chart-wrapper").First().ChildNodes[1].InnerText, "\\\"", "");
            string[] foilDateArray = Regex.Match(foilJavaScriptString, @"labels:\[([^\]]+)]").Groups[1].Value.Split(',');
            string[] foilSellArray = Regex.Match(foilJavaScriptString, @"Avg\. Sell Price,data:\[([^\]]+)]").Groups[1].Value.Split(',');
            string[] foilTrendArray = Regex.Match(foilJavaScriptString, @"Price Trend,data:\[([^\]]+)]").Groups[1].Value.Split(',');

            for (int i = 0; i < dateArray.Length; i++)
            {
                priceGuide = new PriceGuide();
                if (DateTime.TryParse(dateArray[i], out DateTime parsedDate))
                {
                    priceGuide.FetchDate = parsedDate.Date;
                }
                else
                {
                    throw new ScrapingException("Could not parse Date from graph");
                }
                if (decimal.TryParse(sellArray[i], out decimal parsedSell))
                {
                    priceGuide.Sell = parsedSell;
                }
                else
                {
                    throw new ScrapingException("Could not parse Sell from graph");
                }
                if (decimal.TryParse(trendArray[i], out decimal parsedTrend))
                {
                    priceGuide.Trend = parsedTrend;
                }
                else
                {
                    throw new ScrapingException("Could not parse Trend from graph");
                }
                priceGuides.Add(priceGuide);
            }

            for (int i = 0; i < foilDateArray.Length; i++)
            {
                priceGuide = new PriceGuide();
                if (DateTime.TryParse(foilDateArray[i], out DateTime parsedFoilDate))
                {
                    priceGuide.FetchDate = parsedFoilDate.Date;
                }
                else
                {
                    throw new ScrapingException("Could not parse Date from graph");
                }
                if (decimal.TryParse(sellArray[i], out decimal parsedFoilSell))
                {
                    priceGuide.FoilSell = parsedFoilSell;
                }
                else
                {
                    throw new ScrapingException("Could not parse FoilSell from graph");
                }
                if (decimal.TryParse(foilTrendArray[i], out decimal parsedFoilTrend))
                {
                    priceGuide.FoilTrend = parsedFoilTrend;
                }
                else
                {
                    throw new ScrapingException("Could not parse FoilTrend from graph");
                }
                priceGuides.Add(priceGuide);
            }

            return priceGuides;
        }

        #endregion




    }
}


