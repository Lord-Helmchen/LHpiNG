using HtmlAgilityPack;
using LHpiNG.Web;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    public class ExpansionList : IEnumerable
    {
        public List<Expansion> Expansions { get; set; }
        public Uri Url { get; set; }
        public DateTime FetchedOn { get; set; }

        public ExpansionList()
        {
            this.Url = new Uri("https://sandbox.cardmarket.com/en/Magic/Expansions");
            this.Expansions = new List<Expansion>();
        }



        public ExpansionList ImportFromAPI()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Expansions).GetEnumerator();
        }
        public void Add(Expansion expansion)
        {
            this.Expansions.Add(expansion);
        }
    }
}
