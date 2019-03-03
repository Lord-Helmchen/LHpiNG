using LHpiNG.Album;
using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LHpiNG.Util;

namespace LHpiNG.Maps
{
    public class CardProductMap: IEquatable<CardProductMap>
    {
        public byte[] CardUid { get; set; }
        public Card Card { get; set; }
        public byte[] ProductUid { get; set; }
        public Product Product { get; set; }


        public CardProductMap() : base() { }
        public CardProductMap(Card card, Product product) : base()
        {
            Card = card;
            CardUid = card.Uid;
            Product = product;
            ProductUid = product.Uid;
        }

        bool IEquatable<CardProductMap>.Equals(CardProductMap other)
        {
            if (other == null) return false;
            return CardUid.ByteArrayCompare(other.CardUid)
                && ProductUid.ByteArrayCompare(other.ProductUid);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as CardProductMap);
        }
        public override int GetHashCode()
        {
            return (CardUid, ProductUid).GetHashCode();
        }

    }
}
