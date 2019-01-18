using System;
using System.Collections.Generic;
using SQLite;

namespace LHpiNG.Cardmarket
{
    public class ExpansionEntity
    {
        [PrimaryKey]
        public int IdExpansion { get; set; }                        // Expansion's ID
        [Indexed]
        public string EnName { get; set; }                          // Expansion's name in English
        [Ignore]
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the expansion's name
        [Indexed]
        public string Abbreviation { get; set; }                    // the expansion's abbreviation
        [Ignore]
        public int? Icon { get; set; }                               // Index of the expansion's icon
        public string ReleaseDate { get; set; }                     // the expansion's release date
        public bool? IsReleased { get; set; }                        // true|false; a flag if the expansion is released yet
        [Ignore]
        public int IdGame { get; set; }                             // the game ID the expansion belongs to
        [Ignore]
        public IEnumerable<string> Links { get; set; }              // HATEOAS links

        public ExpansionEntity()
        {
            this.IdGame = 1; //Magic
            this.Links = null;
            this.Icon = null;
        }
    }

}
