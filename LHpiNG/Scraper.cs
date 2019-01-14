using ScrapySharp.Network;
using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG
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
            WebPage result = Browser.NavigateToPage(uri);
            if (HttpStatusCode.OK.Equals(result.RawResponse.StatusCode))
            {
                Console.WriteLine(String.Format("got {0} with status {1}", uri.AbsoluteUri, result.RawResponse.StatusCode));
                return result;
            }
            else
            {
                Exception ex = new Exception(String.Format("unhandled response status: {0} ({1})", result.RawResponse.StatusCode, result.RawResponse.StatusDescription));
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
