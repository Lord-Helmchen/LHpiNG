using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNg.Web
{
    interface IFromCardmarket
    {
        /// <summary>
        /// Fetch available Expansions
        /// </summary>
        /// <returns></returns>
        ExpansionList ImportExpansions();

        IEnumerable<ProductEntity> ImportProductList(ExpansionEntity expansion);

        IEnumerable<ExpansionEntity> ImportAllProductLists(IEnumerable<ExpansionEntity> expansions);

        ProductEntity ImportProduct(ProductEntity product);

        IEnumerable<ProductEntity> ImportAllProducts(ExpansionEntity expansion);
    }
}
