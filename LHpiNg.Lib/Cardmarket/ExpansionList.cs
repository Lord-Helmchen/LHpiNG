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
        public string UrlSuffix { get; set; }
        public DateTime FetchedOn { get; set; }

        public ExpansionList()
        {
            this.UrlSuffix = "/en/Magic/Expansions";
            this.Expansions = new List<Expansion>();
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
