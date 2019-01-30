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

namespace LHpiNG.Web
{
    public sealed class Scraper : IFromCardmarket
    {
        public ScrapingBrowser Browser { get; set; }
        public string UrlPrefix { get; set; }

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
            UrlPrefix = "https://sandbox.cardmarket.com";
        }

        public WebPage FetchPage(Uri uri)
        {
            try
            {
                WebPage result = Browser.NavigateToPage(uri);
                if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode)
                {
                    Console.WriteLine(String.Format("got {0} with status {1}", uri.AbsoluteUri, result.RawResponse.StatusCode));
                    return result;
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

            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlPrefix,expansionList.UrlSuffix)));
            IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("div.expansion-row");

            foreach (HtmlNode node in nodes)
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
                    bool dateParsed = DateTime.TryParse(expansion.ReleaseDate, out DateTime parsedDate);
                    if (dateParsed)
                    {
                        expansion.ReleaseDateTime = parsedDate;
                    }
                    else if (DateTime.TryParse(Regex.Replace(expansion.ReleaseDate, @"^(\d+)[snrt][tdh]", "$1"), out parsedDate))//remove "st" from "1st" etc
                    {
                        expansion.ReleaseDateTime = parsedDate;
                    }
                    else
                    {
                        throw new FormatException("Release Date not parsed");
                    }
                    expansion.IsReleased = parsedDate.Date < DateTime.Now.Date;
                    expansionList.Add(expansion);
                }
                catch// (Exception ex)
                {
                    throw;
                }
            }
            expansionList.FetchedOn = DateTime.Now;

            expansionList = FetchProductsUrlSuffix(expansionList);

            return expansionList;
        }

        public ExpansionList FetchProductsUrlSuffix(ExpansionList expansionList)
        {
            foreach (Expansion expansion in expansionList)
            {
                if (expansion.ProductCount > 0
                    //&& (expansion.EnName == "Ugin's Fate Promos" || expansion.EnName == "Commander 2018")//Debug
                    )
                {
                    WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlPrefix, expansion.UrlSuffix)));
                    IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("a.card");
                    foreach (HtmlNode node in nodes)
                    {
                        if (node.GetAttributeValue("href").Contains("Singles"))
                        {
                            expansion.ProductsUrlSuffix = node.GetAttributeValue("href");
                            break;
                        }
                        throw new Exception("throttling needed ?");

                    }
                }
            }
            return expansionList;
        }

        public IEnumerable<ProductEntity> ImportProductList(ExpansionEntity expansion)
        {
            List<ProductEntity> products = new List<ProductEntity>();

            Uri url = new Uri(String.Concat(this.UrlPrefix, ((Expansion)expansion).UrlSuffix));
            WebPage resultpage = FetchPage(url);

            IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("tbody").FirstOrDefault().ChildNodes;
            foreach (HtmlNode node in nodes)
            {
                ProductEntity product = new ProductEntity
                {
                    EnName = node.ChildNodes[2].InnerText,
                    ExpansionName = expansion.EnName,
                    Website = node.ChildNodes[2].ChildNodes[0].GetAttributeValue("href"),
                    Expansion = expansion
                };
                string rarityString = node.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("data-original-title");
                if (Enum.TryParse(rarityString, true, out Rarity parsedRarity))
                {
                    product.Rarity = parsedRarity;

                }
                products.Add(product);
            }
            products.OrderBy(x => x.EnName);

            return products;
        }

        public Product ImportProduct(ProductEntity product)
        {
            throw new NotImplementedException();
        }

    }
}


