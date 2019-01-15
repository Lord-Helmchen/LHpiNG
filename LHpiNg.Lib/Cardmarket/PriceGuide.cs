using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    class PriceGuide : PriceGuideEntity
    {
        public DateTime FetchDate { get; set; }
        public PriceGuide PreviousPriceGuide { get; set; }
    }
}
