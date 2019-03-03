using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public class Atlas
    {
        public HashSet<SetExpansionMap> SetMaps { get; set; }
        public HashSet<CardProductMap> CardMaps { get; set; }

        public Atlas() : base()
        {
            this.SetMaps = new HashSet<SetExpansionMap>();
            this.CardMaps = new HashSet<CardProductMap>();
        }
    }
}
