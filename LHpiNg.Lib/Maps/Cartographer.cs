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
    public class Cartographer
    {

        public static Atlas CreateMaps(IEnumerable<Set> sets, IEnumerable<Expansion> expansions)
        {
            var atlas = new Atlas();
            foreach (Set set in sets)
            {
                HashSet<SetExpansionMap> setMaps = MapSet(set, expansions);
                atlas.SetMaps.UnionWith(setMaps);
            }
            return atlas;
        }

        private static HashSet<SetExpansionMap> MapSet(Set set, IEnumerable<Expansion> expansions)
        {
            var maps = new HashSet<SetExpansionMap>();
            foreach (Card card in set.Cards)
            {
                Product match = FindMatch(card, expansions);
                if (match != null)
                {
                    SetExpansionMap map = new SetExpansionMap(card.Set, match.Expansion as Expansion);
                    if (maps.None(m => m.ExpansionUid == map.ExpansionUid))
                    {
                        maps.Add(map);
                    }
                }
            }
            return maps;
        }

        private static IEnumerable<Product> FindMatchCandidates(Card card, IEnumerable<Expansion> expansions)
        {
            List<Product> candidates = new List<Product>();
            foreach (Expansion expansion in expansions)
            {
                candidates.AddRange(expansion.Products.Where(p => p.EnName == card.OracleName
                                                            && (int)p.Rarity == (int)card.Rarity
                                                            && p.CollNr == card.CollNr));
            }
            return candidates;
        }

        private static Product FindMatch(Card card, IEnumerable<Expansion> expansions)
        {
            IEnumerable<Product> candidates = FindMatchCandidates(card, expansions);
            switch (candidates.Count())
            {
                case 0:
                case 1:
                    return candidates.SingleOrDefault();
                default:
                    Product selected = null;
                    selected = ManualMatch(card, candidates);
                    //display objects and candidates
                    //manually select correct candidate (or none)
                    return selected;
            }

        }

        private static Product ManualMatch(Card card, IEnumerable<Product> candidates)
        {

            throw new NotImplementedException();
        }
    }
}