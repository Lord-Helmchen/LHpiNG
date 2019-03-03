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
    public class PriceGuide : PriceGuideProEntity, IEquatable<PriceGuide>
    {
        public int Uid { get; set; }
        public DateTime FetchDate { get; set; }
        public Product Product { get; set; }

        public string ProductName { get; set; }
        public string ExpansionName { get; set; }

        public PriceGuide() : base()
        {
        }
        //constructor from API reply
        public PriceGuide(string jsonEntity) : base(jsonEntity)
        {
            throw new NotImplementedException();
        }
        public PriceGuide(PriceGuideEntity entity) : base(entity)
        {
            if (entity is PriceGuide priceGuide)
            {
                Uid = priceGuide.Uid;
                FetchDate = priceGuide.FetchDate;
                Product = priceGuide.Product;
            }
        }

        bool IEquatable<PriceGuide>.Equals(PriceGuide other)
        {
            if (other == null) return false;
            return ProductName == other.ProductName
                && ExpansionName == other.ExpansionName
                && FetchDate.Day.Equals(other.FetchDate.Day);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as PriceGuide);
        }
        public override int GetHashCode()
        {
            return (ProductName, ExpansionName, FetchDate.Day).GetHashCode();
        }
    }
}
