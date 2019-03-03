using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Album;
using LHpiNG.Cardmarket;

namespace LHpiNG.Maps
{
    public class SetExpansionMap
    {
        public string SetTLA { get; set; }
        public Set Set { get; set; }
        public byte[] ExpansionUid { get; set; }
        public Expansion Expansion { get; set; }

        public SetExpansionMap() : base() { }
        public SetExpansionMap(Set set, Expansion expansion): base()
        {
            Set = set;
            SetTLA = set.TLA;
            Expansion = expansion;
            ExpansionUid = expansion.Uid;
        }
    }

}
