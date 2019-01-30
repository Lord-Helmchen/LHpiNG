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

        public IEnumerable<ProductEntity> ImportAllProducts(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public ExpansionList ImportExpansions()
        {
            throw new NotImplementedException();
        }

        public ProductEntity ImportProduct(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductEntity> ImportProductList(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        IEnumerable<ExpansionEntity> IFromCardmarket.ImportAllProductLists(IEnumerable<ExpansionEntity> expansions)
        {
            throw new NotImplementedException();
        }
    }
}
