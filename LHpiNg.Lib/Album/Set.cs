using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Album
{
    public class Set : IEquatable<Set>
    {
        //identification
        public string TLA { get; set; }     // altKey
        public int Id { get; set; }         // pKey
        public string Name { get; set; }    // index
        //information
        public ICollection<Card> Cards { get; set; }
        //public ICollection<Language> Languages { get; set; }//postponed, many-to-many in EFCore needs join table
        public int? CardCount { get; set; }
        public int? TokenCount { get; set; }
        public int? NontraditionalCount { get; set; }
        public int? InsertCount { get; set; }
        public int? ReplicaCount { get; set; }
        public Foilage Foilage { get; set; }

        bool IEquatable<Set>.Equals(Set other)
        {
            if (other == null) return false;
            return Id == other.Id
                && TLA == other.TLA
                && Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Set);
        }
        public override int GetHashCode()
        {
            return (Id,TLA,Name).GetHashCode();
        }
    }
}