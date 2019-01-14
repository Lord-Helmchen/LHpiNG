using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Album
{
    class CardInfo
    {
        //MA identification
        public string OracleName { get; set; }
        public string Version { get; set; }
        public Album.Set Set { get; set; }
        public Language Language { get; set; }
        //information
        public int CollectorNumber { get; set; }
        public bool HasFoil { get; set; }
        public bool HasNonfoil { get; set; }
        public Album.Rarity Rarity { get; set; }

        public void LoadFromDatabase()
        {
            throw new NotImplementedException();
        }
        public void SaveToDatabase()
        {
            throw new NotImplementedException();
        }
        public void ImportFromAlbum()
        {
            throw new NotImplementedException();
        }
    }
}
