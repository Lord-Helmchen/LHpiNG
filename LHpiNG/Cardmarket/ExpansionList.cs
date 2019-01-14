using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    public class ExpansionList
    {
        IEnumerable<Expansion> Expansions { get; set; }
        Uri Url { get; set; }
        DateTime FetchDate { get; set; }

        public ExpansionList()
        {
            Url = new Uri("https://sandbox.cardmarket.com/en/Magic/Expansions");
        }

        public void LoadFromDatabase()
        {
            throw new NotImplementedException();
        }
        public void SaveToDatabase()
        {
            throw new NotImplementedException();
        }
        public ExpansionList ImportFromWeb(Scraper scraper)
        {
            List<Expansion> expansions = new List<Expansion>();

            WebPage resultpage = scraper.Browser.NavigateToPage(Url);
            IEnumerable<HtmlNode> nodes = resultpage.Html.CssSelect("div.expansion-row");

            foreach (HtmlNode node in nodes)
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
                else if (DateTime.TryParse(Regex.Replace(expansion.ReleaseDate, @"^(\d+)[snrt][tdh]", "$1"), out parsedDate))//remove st from 1st etc
                {
                    expansion.ReleaseDateTime = parsedDate;
                }
                else
                {
                    ;//NOOP
                    throw new FormatException("Release Date not parsed");
                }
                
                expansions.Add(expansion);
            }

            this.Expansions = expansions;
            this.FetchDate = DateTime.Now;
            return this;
        }
        public ExpansionList ImportFromAPI()
        {
            throw new NotImplementedException();
        }
    }
}
