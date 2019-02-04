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

        IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion);

        IEnumerable<ExpansionEntity> ImportProducts(IEnumerable<ExpansionEntity> expansions);

        PriceGuideEntity ImportPriceGuide(ProductEntity product);

        IEnumerable<ProductEntity> ImportPriceGuides(IEnumerable<ProductEntity> products);
    }
}
