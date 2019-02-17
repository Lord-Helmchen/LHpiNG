using LHpiNG.Cardmarket;
using System;
using System.Collections.Generic;

namespace LHpiNG.Web
{
    internal sealed class MkmApi : IFromCardmarket
    {
        private static readonly Lazy<MkmApi> lazy = new Lazy<MkmApi>(() => new MkmApi());

        /// <summary>get singleton instance of Scraper</summary>
        public static MkmApi Instance { get { return lazy.Value; } }

        private MkmApi()
        {
            throw new NotImplementedException();
        }

        public ExpansionList ImportExpansionList()
        {
            throw new NotImplementedException();
        }

        public ICollection<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PriceGuide> ImportPriceGuides(ProductEntity product)
        {
            throw new NotImplementedException();
        }
    }
}
