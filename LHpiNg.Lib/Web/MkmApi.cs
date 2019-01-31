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
        public IEnumerable<Expansion> ImportAllProductLists(IEnumerable<ExpansionEntity> expansions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportAllProductDetails(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public ExpansionList ImportExpansions()
        {
            throw new NotImplementedException();
        }

        public ProductEntity ImportProductDetails(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExpansionEntity> ImportAllProducts(IEnumerable<ExpansionEntity> expansions)
        {
            throw new NotImplementedException();
        }

    }
}
