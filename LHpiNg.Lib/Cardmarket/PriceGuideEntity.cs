using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("PriceGuides")]
    public class PriceGuideEntity
    {
        public decimal Sell { get; set; }                   // Average price of articles ever sold of this product
        public decimal Low { get; set; }                    // Current lowest non-foil price (all conditions)
        public decimal LowexPlus { get; set; }              // Current lowest non-foil price (condition EX and better)
        public decimal LowFoil { get; set; }                // Current lowest foil price
        public decimal Avg { get; set; }                    // Current average non-foil price of all available articles of this product
        public decimal Trend { get; set; }                  // Current trend price

        //for EF
        public int Id { get; set; }
    }
}
