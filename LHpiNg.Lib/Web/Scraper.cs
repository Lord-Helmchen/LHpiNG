using ScrapySharp.Network;
using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Cardmarket;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Text.RegularExpressions;
using LHpiNg.Web;
using ScrapySharp.Html;
using ScrapySharp.Exceptions;
using System.Diagnostics;
using System.Threading;

namespace LHpiNG.Web
{
    public sealed class Scraper : IFromCardmarket
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
                AllowAutoRedirect = true, // Browser has settings you can access in setup
                AllowMetaRedirect = true,
                Language = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-DE")
            };
            UrlServerPrefix = "https://www.cardmarket.com";
            UrlResultsPerPageSuffix = "perSite=50";
            DelayMiliseconds = 100;
        }

        private WebPage FetchPage(Uri uri, int delay = 0)
        {
            DelayMiliseconds = delay > 0 ? delay : DelayMiliseconds;
            //wait for flood protection to calm down
            if (DelayMiliseconds > 0)
            {
                Console.Write($"\rWaiting {DelayMiliseconds / 1000} seconds for flood protection to calm down on {uri.AbsoluteUri}");
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
            try
            {
                WebPage result = Browser.NavigateToPage(uri);
                if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode && result.RawResponse.Body.Length > 0)
                {
                    string msg = String.Format("got {0} with status {1}", uri.AbsoluteUri, result.RawResponse.StatusCode);
                    int msgSpaces = Console.WindowWidth - msg.Length - 3;
                    msgSpaces = (msgSpaces > 0) ? msgSpaces : 0;
                    Console.Write(String.Concat("\r", msg, new String(' ', msgSpaces)));
                    DelayMiliseconds = DelayMiliseconds == 1 ? 1 : DelayMiliseconds / 2;
                    return result;
                }
                else if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode)
                {
                    //recursively work around flood protection
                    DelayMiliseconds = DelayMiliseconds * 2;// remember delay globally
                    return FetchPage(uri, DelayMiliseconds);
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

        public ExpansionList ImportExpansions()
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

                Console.WriteLine();
                return expansion;
            }
            catch// (Exception ex)
            {
                throw;
            }
        }

        private string ScrapeProductsUrlSuffix(Expansion expansion)
        {
            //shortcut nonexisting Singles url
            if (expansion.ProductCount <= 0) // || (expansion.EnName != "Ugin's Fate Promos" || expansion.EnName != "Commander 2018")//Debug
            {
                return null;
            }
            WebPage resultpage = FetchPage(new Uri(String.Concat(UrlServerPrefix, expansion.UrlSuffix, "?", UrlResultsPerPageSuffix)));
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

        public IEnumerable<ExpansionEntity> ImportProducts(IEnumerable<ExpansionEntity> expansions)
        {
            foreach (Expansion expansion in expansions)
            {
                try
                {
                    IEnumerable<ProductEntity> products = new List<ProductEntity>();
                    products = ImportProducts(expansion);
                }
                catch (ScrapingException ex)
                {
                    Console.WriteLine(ex.Message + ": ProductCount=" + expansion.ProductCount + " in " + expansion.EnName);
                }
            }
            return expansions;
        }

        [Obsolete]
        public IEnumerable<ProductEntity> ImportPriceGuides(IEnumerable<ProductEntity> products)
        {
            foreach (Product product in products)
            {
                product.PriceGuide = ImportPriceGuide(product);
                //TODO add to price history (or probably do it in called method)
            }
            return products;
        }

        public IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            if (!(expansion is Expansion))
            {
                throw new ArgumentException("Need Expansion instead of only ExpansionEntity");
            }
            List<ProductEntity> products = new List<ProductEntity>();
            string baseProductsUrl = String.Concat(UrlServerPrefix, ((Expansion)expansion).ProductsUrlSuffix, "?", UrlResultsPerPageSuffix);

            Uri url = new Uri(baseProductsUrl);
            WebPage resultpage = FetchPage(url);

            HtmlNode Table = resultpage.Html.CssSelect("div.table-body").First();
            foreach (HtmlNode row in Table.ChildNodes)
            {
                ProductEntity product = new Product
                {
                    EnName = row.ChildNodes[3].FirstChild.FirstChild.FirstChild.InnerText,
                    ExpansionName = expansion.EnName,
                    Website = row.ChildNodes[3].FirstChild.FirstChild.FirstChild.GetAttributeValue("href"),
                    Expansion = expansion
                };
                string numberString = row.ChildNodes[3].FirstChild.ChildNodes[1].InnerText;
                if (int.TryParse(numberString, out int parsedNumber))
                {
                    product.Number = parsedNumber;
                }
                string rarityString = row.ChildNodes[3].FirstChild.ChildNodes[2].FirstChild.GetAttributeValue("data-original-title");
                if (Enum.TryParse(rarityString, true, out Rarity parsedRarity))
                {
                    product.Rarity = parsedRarity;
                }
                products.Add(product);
            }
            products.OrderBy(x => x.EnName);
            if (products.Count() != ((Expansion)expansion).ProductCount)
            {
                string msg = $"Product Count mismatch (found {products.Count()}, expected {((Expansion)expansion).ProductCount})";
                Console.WriteLine(msg);
                throw new ScrapingException(msg);
            }

            return products.AsEnumerable();
        }

        public PriceGuideEntity ImportPriceGuide(ProductEntity product)
        {
            Uri url = new Uri(String.Concat(this.UrlServerPrefix, product.Website));
            WebPage resultpage = FetchPage(url);

            PriceGuideEntity guideEntity = new PriceGuideEntity();

            HtmlNode infoListNode = resultpage.Html.CssSelect(".info-list-container").First();
            string LowexPlus = Regex.Match(infoListNode.FirstChild.ChildNodes[11].InnerText, @"^(\d+[.,]\d+) [(&#x20AC)€$]").Groups[1].Value;
            if (decimal.TryParse(LowexPlus, out decimal LowexPlusParsed, System.Globalization.NumberStyles.AllowDecimalPoint)){
                guideEntity.LowexPlus = LowexPlusParsed;
            }
            string trend = Regex.Match(infoListNode.FirstChild.ChildNodes[13].FirstChild.InnerText, @"^(\d+[.,]\d+) [(&#x20AC)€$]").Groups[1].Value;
            if (decimal.TryParse(trend, out decimal trendParsed))
            {
                guideEntity.Trend = trendParsed;
            }
            HtmlNode chartWrapper = resultpage.Html.CssSelect(".chart-wrapper").First();
            string javaScriptString = chartWrapper.ChildNodes[2].InnerText;


            guideEntity.Sell = 0;


            if (!(product is Product))
            {
                // only attach a PriceGuide(Pro)Entity
            }
            else
            {
                //update Product.PriceGuides history as well
            }

            throw new NotImplementedException();
        }


    }
}


