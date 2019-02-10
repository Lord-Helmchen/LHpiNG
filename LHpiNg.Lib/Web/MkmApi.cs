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

        public IEnumerable<ProductEntity> ImportProducts(ExpansionEntity expansion)
        {
            throw new NotImplementedException();
        }

        public PriceGuideEntity ImportPriceGuide(ProductEntity product)
        {
            throw new NotImplementedException();
        }

        public IList<PriceGuide> ImportPriceGuides(ProductEntity product)
        {
            IList<PriceGuide> priceGuides = new List<PriceGuide>();
            PriceGuide priceGuide = new PriceGuide(this.ImportPriceGuide(product))
            {
                IdProduct = product.IdProduct,
                FetchDate = DateTime.Now
            };
            PriceGuide oldGuide = product.PriceGuide as PriceGuide;
            if ((oldGuide).FetchDate.Date < priceGuide.FetchDate)
            {
                priceGuide.PreviousPriceGuide = oldGuide;
                priceGuides.Add(oldGuide);
            }
            priceGuides.Add(priceGuide);

            return priceGuides;
        }

    }
}
