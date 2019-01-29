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

        IEnumerable<ProductEntity> ImportProductsByExpansion(ExpansionEntity expansion);

        Product ImportProduct(ProductEntity product);
    }
}
