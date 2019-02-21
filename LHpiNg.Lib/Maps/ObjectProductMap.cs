using LHpiNG.Album;
using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public class ObjectProductMap
    {
        public int AlbumObjectUid { get; set; }
        public AlbumObject AlbumObject { get; set; }
        public int ProductUid { get; set; }
        public Product Product { get; set; }
    }
}
