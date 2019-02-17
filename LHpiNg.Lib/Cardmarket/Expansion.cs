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
        public DateTime? ReleaseDateTime { get; set; }
        public ICollection<Product> Products { get; set; }
        public string UrlSuffix { get; set; }
        public string ProductsUrlSuffix { get; set; }

        public Expansion() : base()
        {
            Products = new List<Product>();
        }
        //constructor from API reply
        public Expansion(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        //copy constructor
        public Expansion(ExpansionEntity entity) : base(entity)
        {
            if (entity is Expansion expansion)
            {
                ProductCount = expansion.ProductCount;
                ReleaseDateTime = expansion.ReleaseDateTime;
                Products = expansion.Products;
                UrlSuffix = expansion.UrlSuffix;
                ProductsUrlSuffix = expansion.ProductsUrlSuffix;
            }
        }
    }
}
