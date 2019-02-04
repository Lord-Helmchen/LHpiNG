using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("Products")]
    public class Product : ProductEntity
    {
        // keep price history
        public IList<PriceGuide> PriceGuides { get; set; }

        public Product() : base()
        {
        }
        //constructor from API reply
        public Product(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public Product(ProductEntity entity) : base(entity)
        {
            //just use copy constructor from base class
        }
    }
}
