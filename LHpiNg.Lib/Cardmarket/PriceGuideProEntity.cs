using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHpiNG.Cardmarket
{
    [Table("PriceGuides")]
    public class PriceGuideProEntity : PriceGuideEntity
    {
        public int? IdProduct { get; set; }         //The product ID
        public decimal? GermanProLow { get; set; }  //The lowest sell price from German professional sellers
        public decimal? Suggested { get; set; }     //A suggested sell price for professional users, determined by an internal algorithm; this algorithm will not be made public
        public decimal? FoilSell { get; set; }      //The average sell price as shown in the chart at the website for foils
        public decimal? FoilTrend { get; set; }     //The trend price as shown at the website(and in the chart) for foils

        public PriceGuideProEntity() : base()
        {

        }
        //constructor from API reply
        public PriceGuideProEntity(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
    }
}
