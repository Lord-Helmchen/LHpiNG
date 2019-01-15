using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    class ExpansionEntity
    {
        public int IdExpansion { get; set; }                        // Expansion's ID
        public string EnName { get; set; }                          // Expansion's name in English
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the expansion's name
        public string Abbreviation { get; set; }                    // the expansion's abbreviation
        public int Icon { get; set; }                               // Index of the expansion's icon
        public string ReleaseDate { get; set; }                   // the expansion's release date
        public bool IsReleased { get; set; }                        // true|false; a flag if the expansion is released yet
        public int IdGame { get; set; }                             // the game ID the expansion belongs to
        public IEnumerable<string> Links { get; set; }              // HATEOAS links
    }
}
