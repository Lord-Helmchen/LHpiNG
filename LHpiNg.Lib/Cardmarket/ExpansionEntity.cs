using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LHpiNG.Cardmarket
{
    [Table("Expansions")]
    public class ExpansionEntity : IEquatable<ExpansionEntity>
    {
        public int? IdExpansion { get; set; }                       // Expansion's ID
        //[Key, Column("Name")]
        public string EnName { get; set; }                          // Expansion's name in English
        [NotMapped]
        public IEnumerable<LocalizationEntity> Localization { get; set; } // localization entities for the expansion's name
        public string Abbreviation { get; set; }                    // the expansion's abbreviation
        [NotMapped]
        public int? Icon { get; set; }                              // Index of the expansion's icon
        public string ReleaseDate { get; set; }                     // the expansion's release date
        public bool? IsReleased { get; set; }                       // true|false; a flag if the expansion is released yet
        [NotMapped]
        public int IdGame { get; set; }                             // the game ID the expansion belongs to
        [NotMapped]
        public IEnumerable<string> Links { get; set; }              // HATEOAS links

        public byte[] Uid { get; set; }

        public ExpansionEntity()
        {
            this.IdGame = 1; //Magic
            this.Links = Enumerable.Empty<string>();//not used yet
            this.Icon = null;//not relevant
            this.Localization = Enumerable.Empty<LocalizationEntity>();
        }
        //constructor from API reply
        public ExpansionEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public ExpansionEntity(ExpansionEntity entity)
        {
            IdExpansion = entity.IdExpansion;
            EnName = entity.EnName;
            Localization = entity.Localization;
            Abbreviation = entity.Abbreviation;
            Icon = entity.Icon;
            ReleaseDate = entity.ReleaseDate;
            IsReleased = entity.IsReleased;
            IdGame = entity.IdGame;
            Links = entity.Links;
            Uid = entity.Uid;
        }

        public bool Equals(ExpansionEntity other)
        {
            if (other == null) return false;
            return EnName == other.EnName;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as ExpansionEntity);
        }
        public override int GetHashCode()
        {
            return (EnName).GetHashCode();
        }

    }

}
