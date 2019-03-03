using LHpiNG.Album;
using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Maps
{
    public class CardProductMap
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
    }
}
