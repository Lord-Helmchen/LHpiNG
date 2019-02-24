using LHpiNG.Album;
using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public static class Cartographer
    {

        public static Product FindMatch(AlbumObject albumObject, IEnumerable<Expansion> expansions)
        {
            List<Product> candidates = new List<Product>();
            foreach (Expansion expansion in expansions)
            {
                candidates.AddRange(expansion.Products.Where(p => p.EnName == albumObject.OracleName
                                                            && (int)p.Rarity == (int)albumObject.Rarity
                                                            && p.CollNr == albumObject.CollNr
                                                            ));
            }
            ;
            return candidates.SingleOrDefault();
        }
    }
}
