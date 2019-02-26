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
        public byte[] AlbumObjectUid { get; set; }
        public AlbumObject AlbumObject { get; set; }
        public byte[] ProductUid { get; set; }
        public Product Product { get; set; }
    }
}
