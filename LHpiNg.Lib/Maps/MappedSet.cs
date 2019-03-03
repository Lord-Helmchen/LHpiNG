using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public class MappedSet : Album.Set
    {
        ICollection<Cardmarket.Expansion> Expansions { get; set; }
    }
}
