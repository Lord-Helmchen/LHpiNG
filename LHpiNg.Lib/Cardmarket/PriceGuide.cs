using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LHpiNG.Cardmarket
{
    public class PriceGuide : PriceGuideEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime FetchDate { get; set; }
        public PriceGuide PreviousPriceGuide { get; set; }
    }
}
