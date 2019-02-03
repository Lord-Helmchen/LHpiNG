using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    [Table("PriceGuides")]
    public class PriceGuide : PriceGuideProEntity
    {
        public DateTime FetchDate { get; set; }

        public int PreviousPriceGuidUid { get; set; }
        [ForeignKey("PreviousPriceGuidUid")]
        public PriceGuide PreviousPriceGuide { get; set; }

        // EF rereference navigation property
        [ForeignKey("Product"), Column(Order = 1)]
        public string ProductName { get; set; }             // Product's English name
        [ForeignKey("Product"), Column(Order = 2)]
        public string ExpansionName { get; set; }           // Expansion's name 

        public Product Product { get; set; }

    }
}
