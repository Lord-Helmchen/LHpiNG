using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Album
{
    class InventoryCard
    {
        public AlbumObject Card { get; set; }
        public int OwnedFoil { get; set; }
        public int OwnedNonfoil { get; set; }
        public int Bought { get; set; }
        public decimal PriceBought { get; set; }
        public int Sold { get; set; }
        public decimal PriceSold { get; set; }

    }
}
