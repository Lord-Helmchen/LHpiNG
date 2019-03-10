using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Album;
using LHpiNG.Cardmarket;
using LHpiNG.Util;

namespace LHpiNG.Maps
{
    public class SetExpansionMap: IEquatable<SetExpansionMap>
    {
        public string SetTLA { get; set; }
        public Set Set { get; set; }
        public byte[] ExpansionUid { get; set; }
        public Expansion Expansion { get; set; }

        public SetExpansionMap() : base() { }
        public SetExpansionMap(Set set, Expansion expansion): base()
        {
            Set = set;
            SetTLA = set.TLA;
            Expansion = expansion;
            ExpansionUid = expansion.Uid;
        }

        public bool Equals(SetExpansionMap other)
        {
            if (other == null) return false;
            return SetTLA == other.SetTLA
                && ExpansionUid.ByteArrayCompare(other.ExpansionUid);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as SetExpansionMap);
        }
        public override int GetHashCode()
        {
            return (SetTLA, ExpansionUid).GetHashCode();
        }
    }

}
