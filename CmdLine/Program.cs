using LHpiNg.Web;
using LHpiNG.Cardmarket;
using LHpiNG.db;
using LHpiNG.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LHpiNG
{
    class Program
    {
        static void Main(string[] args)
        {
            Importer importer = new Importer();
            EFContext database = new SQLContext();

            ExpansionList expansionList = new ExpansionList();

            Console.WriteLine("Program started!");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t0 - Print expansionList.Length");
            Console.WriteLine("\t1 - Load Expansions from DB");
            Console.WriteLine("\t2 - Save Expansions to Database");
            Console.WriteLine("\t3 - Fetch Expansions from Web");
            Console.WriteLine("\t4 - null expansionList");

            Console.WriteLine("\t5 - test product list scraping");
            Console.WriteLine("\t6 - test priceguide scraping");

            Console.WriteLine("\t9 - variable Test(...) method");
            Console.WriteLine("\tq - quit");


            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "0"://noop
                        Console.WriteLine(String.Format("{0} Expansions in List", expansionList.Expansions.Count));
                        break;
                    case "1":
                        expansionList = LoadExpansion(database);
                        Console.WriteLine(String.Format("{0} Expansions loaded", expansionList.Expansions.Count));
                        break;
                    case "2":
                        SaveExpansionList(expansionList, database);
                        Console.WriteLine(String.Format("{0} Expansions saved", expansionList.Expansions.Count));
                        break;
                    case "3":
                        expansionList = ScrapeExpansionList(importer);
                        Console.WriteLine(String.Format("{0} Expansions scraped", expansionList.Expansions.Count));
                        break;
                    case "4"://null list
                        expansionList = new ExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions in List", expansionList.Expansions.Count));
                        break;
                    case "5":
                        Expansion expansion = TestScrapeProducts(importer, expansionList);
                        Console.WriteLine(String.Format("{0} Products scraped", expansion.Products.Count()));
                        break;
                    case "6":
                        //ProductEntity prod = TestScrapePrice(importer, expansionList);
                        break;
                    case "7":
                        break;
                    case "8":
                        break;
                    case "9":
                        Test(expansionList, importer);
                        Console.WriteLine(String.Format("Test() done"));
                        break;
                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine(String.Format("invalid choice"));
                        break;
                }
            }

            //Console.WriteLine("Press any key to close the window!");
            //Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static void Test(ExpansionList expansionList, Importer importer)
        {
          //  expansionList.Expansions = importer.ImportProducts(expansionList.Expansions).Cast<Expansion>().ToList();
        }

        private static ExpansionList LoadExpansion(EFContext database)
        {
            ExpansionList expansions = database.LoadExpansionList();
            return expansions;
        }

        private static void SaveExpansionList(ExpansionList expansionList, EFContext database)
        {
            database.SaveExpansionList(expansionList);
        }

        private static ExpansionList ScrapeExpansionList(Importer importer)
        {
            ExpansionList expansions = importer.ImportExpansionList();
            return expansions;
        }

        // "correct" implementation
        private static ExpansionList ScrapeProducts(Importer importer, ExpansionList expansionList)
        {
            importer.ImportProducts(expansionList);
            return expansionList;
        }
        //shorten expansion list an call ScrapeProducts(...)
        private static Expansion TestScrapeProducts(Importer importer, ExpansionList expansionList)
        {
            expansionList.Expansions = expansionList.Expansions.Where(x => x.EnName == "Ugin's Fate Promos").ToList();
            expansionList = ScrapeProducts(importer, expansionList);
            return expansionList.Expansions.SingleOrDefault(x => x.EnName == "Ugin's Fate Promos");
        }

        //private static ProductEntity TestScrapePrice(Importer importer, ExpansionList expansionList)
        //{
            //Expansion expansion = expansionList.Expansions.SingleOrDefault(x => x.EnName == "Ugin's Fate Promos");
            //ProductEntity product = expansion.Products.Where(x => x.EnName == "Ghostfire Blade").Single();
            //PriceGuideEntity priceGuide = scraper.ImportPriceGuide(product);
            //product.PriceGuide = priceGuide;
            //return product;

        //}

        [Obsolete]
        private static void TestDb(EFContext database)
        {
            var expansion = new Expansion
            {
                Abbreviation = "FOO",
                EnName = "Foobar",
                IdExpansion = 13,
                ProductCount = 69
            };

            database.AddOrUpdateExpansion(expansion);
        }

        [Obsolete]
        private static int CheckProductSuffixScrapingNeeded(ExpansionList all)
        {
            IEnumerable<Expansion> almostall = all.Expansions.Where(e => e.ProductCount > 0);
            IEnumerable<Expansion> equal = almostall.Where(e => Regex.Replace(e.UrlSuffix, @"^.*[^/]/", "/") == Regex.Replace(e.ProductsUrlSuffix, @"^.*[^/]/", "/"));
            int equalCount=  equal.Count();
            IEnumerable<Expansion> different = almostall.Where(e => Regex.Replace(e.UrlSuffix, @"^.*[^/]/", "/") != Regex.Replace(e.ProductsUrlSuffix, @"^.*[^/]/", "/"));
            int diffCount = different.Count();

            return diffCount;
        }
    }
}
