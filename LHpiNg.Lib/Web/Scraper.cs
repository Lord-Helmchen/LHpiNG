﻿using ScrapySharp.Network;
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

namespace LHpiNG.Web
{
    public sealed class Scraper : IFromCardmarket
    {
        private ScrapingBrowser Browser { get; set; }
        private string UrlPrefix { get; set; }

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

        private WebPage FetchPage(Uri uri)
        {
            try
            {
                WebPage result = Browser.NavigateToPage(uri);
                if ((int)HttpStatusCode.OK == result.RawResponse.StatusCode)
                {
                    string msg = String.Format("got {0} with status {1}", uri.AbsoluteUri, result.RawResponse.StatusCode);
                    int msgSpaces = Console.WindowWidth - msg.Length - 3;
                    msgSpaces = (msgSpaces > 0) ? msgSpaces: 0;
                    Console.Write(String.Concat("\r", msg, new String(' ', msgSpaces)));
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

            Console.WriteLine();
            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlPrefix, expansionList.UrlSuffix)));
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

            expansionList = ImportProductsUrlSuffix(expansionList);

            return expansionList;
        }

        private ExpansionList ImportProductsUrlSuffix(ExpansionList expansionList)
        {
            Console.WriteLine();
            foreach (Expansion expansion in expansionList)
            {
                if (expansion.ProductCount > 0) //&& (expansion.EnName == "Ugin's Fate Promos" || expansion.EnName == "Commander 2018")//Debug
                {
                    expansion.ProductsUrlSuffix = ImportProductsUrlSuffix(expansion);
                    if (expansion.ProductsUrlSuffix == null)
                    {
                        throw new Exception("throttling needed ?");//should have reached break;
                    }
                }
            }
            return expansionList;
        }

        private string ImportProductsUrlSuffix(Expansion expansion)
        {
            WebPage resultpage = FetchPage(new Uri(String.Concat(this.UrlPrefix, expansion.UrlSuffix)));
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

        public IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            if (!(expansion is Expansion))
            {
                throw new ArgumentException("Need Expansion instead of only ExpansionEntity");
            }
            List<ProductEntity> products = new List<ProductEntity>();

            Uri url = new Uri(String.Concat(this.UrlPrefix, ((Expansion)expansion).ProductsUrlSuffix));
            Console.WriteLine();
            WebPage resultpage = FetchPage(url);

            HtmlNode Table = resultpage.Html.CssSelect("table.MKMTable").First();
            foreach (HtmlNode row in Table.SelectNodes("tbody/tr"))
            {
                ProductEntity product = new Product
                {
                    EnName = row.ChildNodes[2].InnerText,
                    ExpansionName = expansion.EnName,
                    Website = row.ChildNodes[2].ChildNodes[0].GetAttributeValue("href"),
                    Expansion = expansion
                };
                string rarityString = row.ChildNodes[3].FirstChild.FirstChild.GetAttributeValue("data-original-title");
                if (Enum.TryParse(rarityString, true, out Rarity parsedRarity))
                {
                    product.Rarity = parsedRarity;

                }
                products.Add(product);
            }
            products.OrderBy(x => x.EnName);

            return products.AsEnumerable();
        }

        public IEnumerable<ExpansionEntity> ImportAllProducts(IEnumerable<ExpansionEntity> expansions)
        {
            if (!(expansions is IEnumerable<Expansion>))
            {
                throw new ArgumentException("Need Expansion instead of only ExpansionEntity");
            }
            foreach (Expansion expansion in expansions)
            {
                try
                {
                    IEnumerable<ProductEntity> products = new List<ProductEntity>();
                    if (expansion.ProductCount > 0)
                    {
                        products = ImportProducts(expansion);
                    }
                    if (products.Count() != expansion.ProductCount)
                    {
                        throw new ScrapingException("Product Count mismatch (found" + products.Count());
                    }
                }
                catch (ScrapingException ex)
                {
                    Console.WriteLine(ex.Message + ": ProductCount=" + expansion.ProductCount + " in " + expansion.EnName);
                }

            }
            return expansions;
        }

        public ProductEntity ImportProductDetails(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportAllProductDetails(ExpansionEntity expansion)
        {
            if (!(expansion is Expansion))
            {
                throw new ArgumentException("Need Expansion instead of only ExpansionEntity");
            }
            List<Product> products = new List<Product>();
            foreach (Product expansionProduct in ((Expansion)expansion).Products)
            {
                Product product = ImportProductDetails(expansionProduct) as Product;
                products.Add(product);
            }
            return products;
        }
    }
}


