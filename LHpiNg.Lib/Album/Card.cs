using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Album
{
    public class Card : IEquatable<Card>
    {
        public byte[] Uid { get; set; }             // altKey
        //MA identification
        public string OracleName { get; set; }      // pKey.1
        public string Version { get; set; }         // pKey.2
        public string SetTLA { get; set; }          // pKey.3
        public Set Set { get; set; }
        public string LanguageTLA { get; set; }     // pKey.5
        public Language Language { get; set; }
        public ObjectType ObjectType { get; set; }  // pKey.4
        //information
        public string Number { get; set; }
        public int? CollNr { get; set; }
        public Foilage Foilage { get; set; }
        public string RarityString { get; set; }
        public Rarity Rarity { get; set; }
        public string Color { get; set; }//enum?
        [NotMapped]
        public string Cost { get; set; }
        [NotMapped]
        public string PowerToughness { get; set; }
        [NotMapped]
        public string OracleType { get; set; }//enum?
        [NotMapped]
        public string Legality { get; set; }//enum?
        [NotMapped]
        public string Artist { get; set; }
        [NotMapped]
        public string Border { get; set; }//enum?
        [NotMapped]
        public string Copyright { get; set; }
        [NotMapped]
        public string Comment { get; set; }
        [NotMapped]
        public string PrintedType { get; set; }
        [NotMapped]
        public string PrintedName { get; set; }
        [NotMapped]
        public string PriceRegular { get; set; }
        [NotMapped]
        public string PriceFoil { get; set; }
        [NotMapped]
        public string Rating { get; set; }

        bool IEquatable<Card>.Equals(Card other)
        {
            if (other == null) return false;
            return OracleName == other.OracleName
                && Version == other.Version
                && SetTLA == other.SetTLA
                && ObjectType == other.ObjectType
                && LanguageTLA == other.LanguageTLA;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Card);
        }
        public override int GetHashCode()
        {
            return (OracleName, Version, SetTLA, ObjectType, LanguageTLA).GetHashCode();
        }
    }
}
