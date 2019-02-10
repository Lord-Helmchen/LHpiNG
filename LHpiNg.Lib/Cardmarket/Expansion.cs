using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    [Table("Expansions")]
    public class Expansion : ExpansionEntity
    {
        public int? ProductCount { get; set; }
        public DateTime? ReleaseDateTime
        {
            get;
            set;
        }
        //        public DateTime? ReleaseDateTime { get => ReleaseDateTime.Value.Date; set => ReleaseDateTime = value; }
        public IEnumerable<Product> Products { get; set; }
        public string UrlSuffix { get; set; }
        public string ProductsUrlSuffix { get; set; }

        public Expansion() : base()
        {
        }
        //constructor from API reply
        public Expansion(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public Expansion(ExpansionEntity entity) : base(entity)
        {
            //just use copy constructor from base class
        }
    }
}
