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
using LHpiNg.MAFiles;

namespace LHpiNG
{
    class Program
    {
        public static Importer Importer { get; set; }
        public static EFContext Database { get; set; }
        public static MAReader Reader { get; set; }

        public static ExpansionList ExpansionList { get; set; }
        public static IEnumerable<Album.Language> AlbumLanguages { get; set; }
        public static IEnumerable<Album.Set> AlbumSets { get; set; }
        public static IEnumerable<Album.AlbumObject> AlbumObjects {get;set;}

        static void Main(string[] args)
        {

            Database = new SQLContext();
            ExpansionList = new ExpansionList();

            Console.WriteLine("Program started!");
            MainMenu();

            //Console.WriteLine("Press any key to close the window!");
            //Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("choose modeule:");
            Console.WriteLine("\t1 - NoOp");
            Console.WriteLine("\t2 - Market");
            Console.WriteLine("\t3 - Album");
            Console.WriteLine("\t4 - Mapping");
            Console.WriteLine("\t9 - Test()");
            Console.WriteLine("\t0 - quit");
        }

        private static void MainMenu()
        {
            PrintMainMenu();
            bool back = false;
            while (!back)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("NoOp");
                        break;
                    case "2":
                        MarketMenu();
                        PrintMainMenu();
                        break;
                    case "3":
                        AlbumMenu();
                        PrintMainMenu();
                        break;
                    case "4":
                        Console.WriteLine("Mapping Menu not implemented yet!");
                        break;
                    case "9":
                        Test();
                        Console.WriteLine(String.Format("Test() done"));
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine(String.Format("invalid choice"));
                        break;
                }
            }
        }

        private static void PrintMarketMenu()
        {
            Console.WriteLine("\nMarket Menu");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t1 - Print expansionList.Length");
            Console.WriteLine("\t2 - Load Expansions from DB");
            Console.WriteLine("\t3 - Save Expansions to Database");
            Console.WriteLine("\t4 - Fetch Expansions from Web");
            Console.WriteLine("\t5 - null expansionList");

            Console.WriteLine("\t6 - test product list scraping");
            Console.WriteLine("\t7 - test priceguide scraping");

            Console.WriteLine("\t8 - reduce expansionList to debug-worthy cases");
            Console.WriteLine("\t0 - return");
        }

        private static void MarketMenu()
        {
            PrintMarketMenu();

            Importer = Importer ?? new Importer();

            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "1"://noop
                        Console.WriteLine(String.Format("{0} Expansions in List", ExpansionList.Expansions.Count));
                        break;
                    case "2":
                        ExpansionList = Database.LoadExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions loaded", ExpansionList.Expansions.Count));
                        break;
                    case "3":
                        Database.SaveExpansionList(ExpansionList);
                        Console.WriteLine(String.Format("{0} Expansions saved", ExpansionList.Expansions.Count));
                        break;
                    case "4":
                        ExpansionList = Importer.ImportExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions scraped", ExpansionList.Expansions.Count));
                        break;
                    case "5"://null list
                        ExpansionList = new ExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions in List", ExpansionList.Expansions.Count));
                        break;
                    case "6":
                        ExpansionList = CullExpansionList(ExpansionList);
                        ExpansionList = Importer.ImportProducts(ExpansionList);
                        break;
                    case "7":
                        TestScrapePrice(Importer, ExpansionList);
                        break;
                    case "8":
                        CullExpansionList(ExpansionList);
                        Console.WriteLine(String.Format("{0} Expansions selected", ExpansionList.Expansions.Count));
                        break;
                    case "0":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine(String.Format("invalid choice"));
                        break;
                }
            }
        }

        private static void AlbumMenu()
        {
            Console.WriteLine("\nAlbum Menu");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t1 - Load Languages from database");
            Console.WriteLine("\t2 - Read Languages from file");
            Console.WriteLine("\t3 - Save Languages to Database");
            Console.WriteLine("\t4 - Load Sets from database");
            Console.WriteLine("\t5 - Read Sets from file");
            Console.WriteLine("\t6 - Save Sets to Database");
            Console.WriteLine("\t7 - Load Objects from database");
            Console.WriteLine("\t8 - Read Objects from file");
            Console.WriteLine("\t9 - Save Objects to Database");
            Console.WriteLine("\t0 - return");

            Reader = Reader ?? new MAReader();

            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "1":
                        AlbumLanguages = Database.LoadLanguages();
                        break;
                    case "2":
                        AlbumLanguages = Reader.ReadLanguages();
                        break;
                    case "3":
                        Database.SaveLanguages(AlbumLanguages);
                        break;
                    case "4":
                        AlbumSets = Database.LoadSets();
                        break;
                    case "5":
                        AlbumSets = Reader.ReadSets();
                        break;
                    case "6":
                        Database.SaveSets(AlbumSets);
                        break;
                    case "7":
                        AlbumObjects = Database.LoadObjects();
                        break;
                    case "8":
                        AlbumObjects = Reader.ReadObjects();
                        break;
                    case "9":
                        Database.SaveAlbumObjects(AlbumObjects);
                        break;
                    case "0":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine(String.Format("invalid choice"));
                        break;
                }
            }
        }

        private static void Test()
        {
        }

        private static ExpansionList CullExpansionList(ExpansionList expansionList)
        {
            expansionList.Expansions = expansionList.Expansions.Where(
                x => x.EnName == "Ugin's Fate Promos"
                //|| x.EnName == "Explorers of Ixalan"
                ).ToList();
            return expansionList;
        }

        [Obsolete]
        private static Expansion TestScrapeProducts(Importer importer, ExpansionList expansionList)
        {
            expansionList.Expansions = expansionList.Expansions.Where(x => x.EnName == "Ugin's Fate Promos").ToList();
            expansionList = importer.ImportProducts(expansionList);
            return expansionList.Expansions.SingleOrDefault(x => x.EnName == "Ugin's Fate Promos");
        }

        private static ExpansionList TestScrapePrice(Importer importer, ExpansionList expansionList)
        {
            expansionList.Expansions = expansionList.Expansions.Where(x => x.EnName == "Ugin's Fate Promos").ToList();
            expansionList.Expansions.Single().Products = expansionList.Expansions.Single().Products.Where(x => x.EnName == "Ghostfire Blade").ToList();
            importer.ImportPrices(expansionList);
            return expansionList;
        }

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
