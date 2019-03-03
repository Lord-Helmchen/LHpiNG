using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Album
{
    public class Set
    {
        //identification
        public string TLA { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        //information
        public ICollection<Card> Cards { get; set; }
        //public ICollection<Language> Languages { get; set; }//postponed, many-to-many in EFCore needs join table
        public int? CardCount { get; set; }
        public int? TokenCount { get; set; }
        public int? NontraditionalCount { get; set; }
        public int? InsertCount { get; set; }
        public int? ReplicaCount { get; set; }
        public Foilage Foilage { get; set; }
    }
}