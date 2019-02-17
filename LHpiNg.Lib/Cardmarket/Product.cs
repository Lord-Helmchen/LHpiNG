using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("Products")]
    public class Product : ProductEntity
    {
        // keep price history
        public ICollection<PriceGuide> PriceGuides { get; set; }
        public int? CollNr { get; set; }

        public Product() : base()
        {
            PriceGuides = new List<PriceGuide>();
        }
        //constructor from API reply
        public Product(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public Product(ProductEntity entity) : base(entity)
        {
            if (entity is Product product)
            {
                PriceGuides = product.PriceGuides;
                CollNr = product.CollNr;
            }
        }
    }
}
