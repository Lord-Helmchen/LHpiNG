using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    internal interface IFromCardmarket
    {
        /// <summary>
        /// Fetch a list of all available expansions
        /// </summary>
        /// <returns>freshly fetched Expansions</returns>
        ExpansionList ImportExpansionList();

        /// <summary>
        /// Fetch products and attach them to the expansion
        /// </summary>
        /// <param name="expansion"></param>
        /// <returns>products for the expansion</returns>
        ICollection<ProductEntity> ImportProducts(ExpansionEntity expansion);

        /// <summary>
        /// fetch prices for a product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        IEnumerable<PriceGuide> ImportPriceGuides(ProductEntity product);
    }
}
