using System;

namespace LHpiNG.Album
{
    public class Language: IEquatable<Language>
    {
        public int Id { get; set; }         // pKey 
        public string TLA { get; set; }     // altKey
        public string M15Abbr { get; set; }
        public string Name { get; set; }

        public bool Equals(Language other)
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
            return Equals(obj as Language);
        }
        public override int GetHashCode()
        {
            return (Id, TLA, Name).GetHashCode();
        }
    }
}