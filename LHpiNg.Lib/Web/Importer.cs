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
                    IEnumerable<ProductEntity> products = Datasource.ImportProducts(expansion);
                    expansion.Products = products.Cast<Product>();// if that is not good, try = products.ConvertAll(x => new Product(x))
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

        public IEnumerable<Product> ImportPrices(IEnumerable<Product> products)
        {
            foreach (Product product in products)
            {
                product.PriceGuide = Datasource.ImportPriceGuide(product);
            }
            return products;
        }


    }
}
