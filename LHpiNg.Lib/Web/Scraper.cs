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

namespace LHpiNG.Web
{
    public class Scraper
    {
        public ScrapingBrowser Browser { get; set; }

        public Scraper()
        {
            Browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true, // Browser has settings you can access in setup
                AllowMetaRedirect = true,
                Language = System.Globalization.CultureInfo.GetCultureInfoByIetfLanguageTag("en-DE")
            };
        }

        public WebPage FetchPage(Uri uri)
        {
            try
            {
                WebPage result = Browser.NavigateToPage(uri);
                if (HttpStatusCode.OK.Equals(result.RawResponse.StatusCode))
                {
                    Console.WriteLine(String.Format("got {0} with status {1}", uri.AbsoluteUri, result.RawResponse.StatusCode));
                    return result;
                }
                else
                {
                    string message = String.Format("unhandled response status: {0} ({1})", result.RawResponse.StatusCode, result.RawResponse.StatusDescription);
                    Console.WriteLine(message);

                    var exception = new HttpException(result.RawResponse.StatusCode, result.RawResponse.StatusDescription);
                    throw exception;
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExpansionList ImportExpansions()
        {
            ExpansionList expansionList = new ExpansionList();

            WebPage resultpage = this.Browser.NavigateToPage(expansionList.Url);
            IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("div.expansion-row");

            foreach (HtmlNode node in nodes)
            {
                try
                {
                    Expansion expansion = new Expansion
                    {
                        EnName = node.ChildNodes[2].ChildNodes[0].InnerText,
                        ReleaseDate = node.ChildNodes[4].InnerText,
                        UrlSuffix = node.ChildNodes[2].ChildNodes[0].Attributes[0].Value
                    };
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
                    expansionList.Add(expansion);
                }
                catch (Exception ex){
                    throw ex;
                }
            }
            expansionList.FetchedOn = DateTime.Now;

            return expansionList;
        }

        public Product ImportProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}


