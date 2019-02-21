using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("PriceGuides")]
    public class PriceGuideEntity
    {
        public decimal? Sell { get; set; }                   // Average price of articles ever sold of this product
        public decimal? Low { get; set; }                    // Current lowest non-foil price (all conditions)
        public decimal? LowexPlus { get; set; }              // Current lowest non-foil price (condition EX and better)
        public decimal? LowFoil { get; set; }                // Current lowest foil price
        public decimal? Avg { get; set; }                    // Current average non-foil price of all available articles of this product
        public decimal? Trend { get; set; }                  // Current trend price

        public PriceGuideEntity()
        {
        }
        //constructor from API reply
        public PriceGuideEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public PriceGuideEntity(PriceGuideEntity entity)
        {
            Sell = entity.Sell;
            Low = entity.Low;
            LowexPlus = entity.LowexPlus;
            LowFoil = entity.LowexPlus;
            Avg = entity.Avg;
            Trend = entity.Trend;
        }
    }
}
