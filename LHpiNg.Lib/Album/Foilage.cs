using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNg.Album
{
    [Flags]
    public enum Foilage
    {
        Foil = 0b000001,
        Nonfoil= 0b000010,
    }
}