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
    public class PriceGuide : PriceGuideEntity
    {
        public DateTime FetchDate { get; set; }
        public int PreviousPriceGuidUid { get; set; }
        [ForeignKey("Uid")]
        public virtual PriceGuide PreviousPriceGuide { get; set; }
    }
}
