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
    }
}
