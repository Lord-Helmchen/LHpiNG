using LHpiNG.Album;
using LHpiNG.Cardmarket;
using LHpiNG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public static class Cartographer
    {

        private static Product FindMatch(AlbumObject albumObject, IEnumerable<Expansion> expansions)
        {
            IEnumerable<Product> candidates = FindMatchCandidates(albumObject, expansions);
            switch (candidates.Count())
            {
                case 0:
                case 1:
                    return candidates.SingleOrDefault();
                default:
                    Product selected = null;
                    //display objects and candidates
                    //manually select correct candidate (or none)
                    return selected;
            }

        }

        private static IEnumerable<Product> FindMatchCandidates(AlbumObject albumObject, IEnumerable<Expansion> expansions)
        {
            List<Product> candidates = new List<Product>();
            foreach (Expansion expansion in expansions)
            {
                candidates.AddRange(expansion.Products.Where(p => p.EnName == albumObject.OracleName
                                                            && (int)p.Rarity == (int)albumObject.Rarity
                                                            && p.CollNr == albumObject.CollNr));
            }
            return candidates;
        }

        public static List<SetExpansionMap> CreateMaps(IEnumerable<Set> sets, IEnumerable<Expansion> expansions)
        {
            var maps = new List<SetExpansionMap>();
            foreach (Set set in sets)
            {
                foreach (AlbumObject albumObject in set.AlbumObjects)
                {
                    Product match = FindMatch(albumObject, expansions);
                    if (match != null)
                    {
                        SetExpansionMap map = new SetExpansionMap(albumObject.Set, match.Expansion as Expansion);
                        if (maps.None(m => m.ExpansionUid == map.ExpansionUid))
                        {
                            maps.Add(map);
                        }
                    }
                }
            }
            return maps;
        }
    }
}
