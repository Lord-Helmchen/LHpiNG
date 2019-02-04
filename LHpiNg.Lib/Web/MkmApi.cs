using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNg.Web
{
    class MkmApi : IFromCardmarket
    {
        public ExpansionList ImportExpansions()
        {
            throw new NotImplementedException();
        }

        public PriceGuideEntity ImportPriceGuide(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportPriceGuides(IEnumerable<ProductEntity> products)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExpansionEntity> ImportProducts(IEnumerable<ExpansionEntity> expansions)
        {
            throw new NotImplementedException();
        }
    }
}
