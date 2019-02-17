using LHpiNg.Album;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Album
{
    public class AlbumObject
    {
        public int Uid { get; set; }
        //MA identification
        public string OracleName { get; set; }
        public string Version { get; set; }
        public string SetTLA { get; set; }
        public Set Set { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        //information
        public string Number { get; set; }
        public int? CollNr { get; set; }
        public Foilage Foilage { get; set; }
        public Album.Rarity Rarity { get; set; }
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
    }
}
