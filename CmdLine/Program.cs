using LHpiNG.Cardmarket;
using LHpiNG.db;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LHpiNG.Album;
using LHpiNG.Util;
using LHpiNG.Maps;

namespace LHpiNG
{
    class Program
    {
        public static Importer Importer { get; set; }
        public static EFContext Database { get; set; }
        public static FileReader Reader { get; set; }

        public static ExpansionList ExpansionList { get; set; }
        public static IEnumerable<Album.Language> AlbumLanguages { get; set; }
        public static IEnumerable<Album.Set> AlbumSets { get; set; }
        public static IEnumerable<Album.AlbumObject> AlbumObjects { get; set; }
        public static IEnumerable<ObjectProductMap> CardMap { get; set; }
        public static IEnumerable<SetExpansionMap> SetMap { get; set; }

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
            Console.WriteLine("\t5 - Save changes to Database");
            Console.WriteLine("\t6 - Load Sets and Expansions from Database");
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
                        MappingMenu();
                        PrintMainMenu();
                        break;
                    case "5":
                        Database.SaveChanges();
                        break;
                    case "6":
                        ExpansionList = Database.LoadExpansionList();
                        AlbumSets = Database.LoadSets();
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
            Console.WriteLine("\t3 - Sync Expansions to DbSet");
            Console.WriteLine("\t4 - Scrape Expansions from Web");
            Console.WriteLine("\t5 - null expansionList");

            Console.WriteLine("\t6 - Scrape Products from Web");
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
                        //ExpansionList.Expansions = ExpansionList.Expansions.Where(e => e.ProductCount != e.Products.Count()).ToList();
                        //ExpansionList.Expansions = ExpansionList.Expansions.Where(e => e.Products.Any(p => p.Rarity == Cardmarket.Rarity.None)).ToList() ;
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
            Console.WriteLine("\t3 - Sync Languages to DbSet");
            Console.WriteLine("\t4 - Load Sets from database");
            Console.WriteLine("\t5 - Read Sets from file");
            Console.WriteLine("\t6 - Sync Sets to DbSet");
            Console.WriteLine("\t7 - Load Objects from database");
            Console.WriteLine("\t8 - Read Objects from file");
            Console.WriteLine("\t9 - Sync Objects to DbSet");
            Console.WriteLine("\t0 - return");

            Reader = Reader ?? new FileReader();

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

        private static void MappingMenu()
        {
            Console.WriteLine("\nMapping Menu");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t1 - Test Fuzzy Match sets to expansions");
            Console.WriteLine("\t2 - try card mapping");
            Console.WriteLine("\t3 - Sync maps to DbSet");
            Console.WriteLine("\t0 - return");

            Reader = Reader ?? new FileReader();

            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "0":
                        quit = true;
                        break;
                    case "1":
                        FuzzyMatchSets();
                        break;
                    case "2":
                        CreateMaps();
                        break;
                    case "3":
                        Database.ObjectProductMap.AddRange(CardMap);
                        break;
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    default:
                        Console.WriteLine(String.Format("invalid choice"));
                        break;
                }
            }
        }

        private static List<ObjectProductMap> CreateMaps()
        {
            var maps = new List<ObjectProductMap>();
            foreach (Set set in AlbumSets)
            {
                foreach (AlbumObject albumObject in set.AlbumObjects)
                {
                    Product match = Cartographer.FindMatch(albumObject, ExpansionList.Expansions);
                    if (match != null)
                    {
                        ObjectProductMap map = new ObjectProductMap { AlbumObjectUid = albumObject.Uid, ProductUid = match.Uid };
                        maps.Add(map);
                    }
                }
            }
            return maps;
        }

        internal class FuzzyMatch
        {
            internal string Set { get; set; }
            internal string Expansion { get; set; }
            internal int Score { get; set; }
        }
        private static void FuzzyMatchSets()
        {
            List<FuzzyMatch> fuzzyMatches = new List<FuzzyMatch>();
            var sets = AlbumSets;
            var expansions = ExpansionList.Expansions;
            foreach (Set set in AlbumSets)
            {
                foreach (Expansion expansion in ExpansionList)
                {
                    set.Name.FuzzyMatch(expansion.EnName, out int outScore);
                    FuzzyMatch fuzzyMatch = new FuzzyMatch
                    {
                        Set = set.Name,
                        Expansion = expansion.EnName,
                        Score = outScore
                    };
                    fuzzyMatches.Add(fuzzyMatch);
                }
            }
            ;
            var positive = fuzzyMatches.Where(x => x.Score > 50);

        }

        private static void Test()
        {
            Expansion exp = ExpansionList.Expansions.FirstOrDefault();
            var foo = exp.EnName.GetHashCode();
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
            int equalCount = equal.Count();
            IEnumerable<Expansion> different = almostall.Where(e => Regex.Replace(e.UrlSuffix, @"^.*[^/]/", "/") != Regex.Replace(e.ProductsUrlSuffix, @"^.*[^/]/", "/"));
            int diffCount = different.Count();

            return diffCount;
        }
    }
}
