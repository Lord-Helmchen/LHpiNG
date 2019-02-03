using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("Expansions")]
    public class ExpansionEntity
    {
        public int? IdExpansion { get; set; }                        // Expansion's ID
        [Key]
        public string EnName { get; set; }                          // Expansion's name in English
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the expansion's name
        public string Abbreviation { get; set; }                    // the expansion's abbreviation
        public int? Icon { get; set; }                              // Index of the expansion's icon
        public string ReleaseDate { get; set; }                     // the expansion's release date
        public bool? IsReleased { get; set; }                       // true|false; a flag if the expansion is released yet
        public int IdGame { get; set; }                             // the game ID the expansion belongs to
        public IEnumerable<string> Links { get; set; }              // HATEOAS links

        public ExpansionEntity()
        {
            this.IdGame = 1; //Magic
            this.Links = null;//not used yet
            this.Icon = null;//not relevant
        }
        //constructor from API reply
        public ExpansionEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }


    }

}
