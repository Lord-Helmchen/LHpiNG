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
        public string ExpansionEnName { get; set; }
        public Expansion Expansion { get; set; }
    }
}
