using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.db
{
    public class State
    {
        public int Id { get; set; }
        public DateTime ExpansionListFetchDate { get; set; }

        public State()
        {
            Id = 0;
        }
    }
}
