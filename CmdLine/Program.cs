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
        public static FileReader Reader { get; set; }

        public static ExpansionList ExpansionList { get; set; }
        public static IEnumerable<Album.Language> AlbumLanguages { get; set; }
        public static IEnumerable<Album.Set> AlbumSets { get; set; }
        public static IEnumerable<Album.Card> Cards { get; set; }
        public static IEnumerable<CardProductMap> CardMap { get; set; }
        public static IEnumerable<SetExpansionMap> SetMap { get; set; }

        static void Main(string[] args)
        {

            //Database = new SQLContext();
            ExpansionList = new ExpansionList();

            Console.WriteLine("Program started!");
            MainMenu();
        }

        private static void PrintMainMenu()
        {
            Console.WriteLine("\nMain Menu");
            Console.WriteLine("choose modeule:");
            Console.WriteLine("\t1 - NoOp");
            Console.WriteLine("\t2 - Market");
            Console.WriteLine("\t3 - Album");
            Console.WriteLine("\t4 - Mapping");
            Console.WriteLine("\t5 - Load Sets and Expansions from Database");
            Console.WriteLine("\t5 - Save Sets and Expansions to Database");
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
                        using (EFContext database = new SQLContext())
                        {
                            ExpansionList = database.LoadExpansionList();
                            AlbumSets = database.LoadSets();
                        }
                        break;
                    case "6":
                        using (EFContext database = new SQLContext())
                        {
                            database.SaveExpansionList(ExpansionList);
                            database.SaveSets(AlbumSets);
                            database.SaveChanges();
                        }
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
            Console.WriteLine("\t1 - Load Expansions from DB");
            Console.WriteLine("\t2 - Save Expansions to DB");
            Console.WriteLine("\t3 - Scrape Expansions from Web");
            Console.WriteLine("\t4 - Scrape Products from Web");
            Console.WriteLine("\t5 - test priceguide scraping");
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
                    case "1":
                        using (EFContext database = new SQLContext())
                        {
                            ExpansionList = database.LoadExpansionList();
                            Console.WriteLine(String.Format("{0} Expansions loaded", ExpansionList.Expansions.Count));
                        }
                        break;
                    case "2":
                        using (EFContext database = new SQLContext())
                        {
                            database.SaveExpansionList(ExpansionList);
                            database.SaveChanges();
                            Console.WriteLine(String.Format("{0} Expansions saved", ExpansionList.Expansions.Count));
                        }
                        break;
                    case "3":
                        ExpansionList = Importer.ImportExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions scraped", ExpansionList.Expansions.Count));
                        break;
                    case "4":
                        ExpansionList = Importer.ImportProducts(ExpansionList);
                        break;
                    case "5":
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
            Console.WriteLine("\t1 - Read Languages from file");
            Console.WriteLine("\t2 - Read Sets from file");
            Console.WriteLine("\t8 - Read Objects from file");
            Console.WriteLine("\t4 - Save Languages to DB");
            Console.WriteLine("\t5 - Save Sets to DB");
            Console.WriteLine("\t6 - Save Objects to DB");
            Console.WriteLine("\t0 - return");

            Reader = Reader ?? new FileReader();

            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "1":
                        AlbumLanguages = Reader.ReadLanguages();
                        break;
                    case "2":
                        AlbumSets = Reader.ReadSets();
                        break;
                    case "3":
                        Cards = Reader.ReadCards();
                        break;
                    case "4":
                        using (EFContext database = new SQLContext())
                        {
                            database.SaveLanguages(AlbumLanguages);
                            database.SaveChanges();
                        }
                        break;
                    case "5":
                        using (EFContext database = new SQLContext())
                        {
                            database.SaveSets(AlbumSets);
                            database.SaveChanges();
                        } break;
                    case "6":
                        using (EFContext database = new SQLContext())
                        {
                            database.SaveCards(Cards);
                            database.SaveChanges();
                        }
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
            Console.WriteLine("\t2 - map expansions to sets");
            Console.WriteLine("\t3 - Save maps to DB");
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
                        Cartographer.CreateMaps(AlbumSets, ExpansionList.Expansions);
                        break;
                    case "3":
                        using (EFContext database = new SQLContext())
                        {
                            database.CardProductMaps.AddRange(CardMap);
                            database.SetExpansionMaps.AddRange(SetMap);
                            database.SaveChanges();
                        }
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
