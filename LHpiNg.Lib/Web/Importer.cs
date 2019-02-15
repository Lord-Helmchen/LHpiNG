using LHpiNG.Cardmarket;
using LHpiNG.Web;
using ScrapySharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNg.Web
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

        public ICollection<Product> ImportPrices(ICollection<Product> products)
        {
            foreach (Product product in products)
            {
                PriceGuide priceGuide = new PriceGuide(Datasource.ImportPriceGuide(product))
                {
                    Product = product,
                    IdProduct = product.IdProduct,

                };
                priceGuide.FetchDate = priceGuide.FetchDate > DateTime.MinValue ? priceGuide.FetchDate : DateTime.Now;

                PriceGuide existingPriceGuide = product.PriceGuide as PriceGuide;

                if (existingPriceGuide == null || priceGuide.FetchDate.Date > existingPriceGuide.FetchDate.Date)
                {
                    priceGuide.PreviousPriceGuide = existingPriceGuide;
                }
                else
                {
                    priceGuide.PreviousPriceGuide = existingPriceGuide.PreviousPriceGuide;
                    product.PriceGuides.Remove(product.PriceGuides.SingleOrDefault(g => g.FetchDate.Date == priceGuide.FetchDate.Date));

                }
                product.PriceGuides.Add(priceGuide);
                product.PriceGuide = priceGuide;
            }
            return products;
        }


    }
}
