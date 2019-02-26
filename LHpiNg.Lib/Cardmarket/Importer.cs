using LHpiNG.Util;
using ScrapySharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Cardmarket
{
    public class Importer
    {
        private IFromCardmarket Datasource { get; set; }

        public Importer(bool noApi = true) : base()
        {
            if (noApi)
            {
                Datasource = Scraper.Instance;
            }
            else
            {
                Datasource = MkmApi.Instance;
            }
        }

        public ExpansionList ImportExpansionList()
        {
            ExpansionList expansionList = Datasource.ImportExpansionList();

            return expansionList;
        }

        public ExpansionList ImportProducts(ExpansionList expansionList)
        {
            expansionList.Expansions = ImportProducts(expansionList.Expansions).ToList<Expansion>();
            return expansionList;
        }

        private IEnumerable<Expansion> ImportProducts(IEnumerable<Expansion> expansions)
        {
            foreach (Expansion expansion in expansions)
            {
                try
                {
                    ICollection<ProductEntity> products = Datasource.ImportProducts(expansion);
                    expansion.Products = products.Cast<Product>().ToList();
                }
                catch (ScrapingException ex)
                {
                    Console.WriteLine(ex.Message + ": ProductCount=" + expansion.ProductCount + " in " + expansion.EnName);
                }
            }
            return expansions;
        }

        public ExpansionList ImportPrices(ExpansionList expansionList)
        {
            foreach (Expansion expansion in expansionList.Expansions)
            {
                expansion.Products = ImportPrices(expansion.Products);
            }
            return expansionList;
        }

        private ICollection<Product> ImportPrices(ICollection<Product> products)
        {
            foreach (Product product in products)
            {
                IEnumerable<PriceGuide> priceGuides  =  Datasource.ImportPriceGuides(product);
                Dictionary<DateTime, PriceGuide> PriceGuidesByDate = product.PriceGuides.ToDictionary(g => g.FetchDate);

                foreach(PriceGuide priceGuide in priceGuides)
                {
                    priceGuide.Product = product;
                    priceGuide.IdProduct = product.IdProduct;
                    if (PriceGuidesByDate.ContainsKey(priceGuide.FetchDate))
                    {
                        PriceGuidesByDate[priceGuide.FetchDate].InjectNonNull(priceGuide);
                    }
                    else
                    {
                        PriceGuidesByDate.Add(priceGuide.FetchDate, priceGuide);
                    }
                }
                product.PriceGuides = PriceGuidesByDate.Values;
            }
            return products;
        }

    }
}
