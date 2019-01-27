using LHpiNG.Cardmarket;
using LHpiNG.db;
using LHpiNG.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG
{
    class Program
    {
        static void Main(string[] args)
        {
            Scraper scraper = Scraper.Instance;
            //EFContext database = new SQLiteContext(SQLiteContext.ConnectionString);
            EFContext database = new SQLContext();
            ExpansionList expansionList = new ExpansionList();

            Console.WriteLine("Program started!");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t1 - Test()");
            Console.WriteLine("\t2 - TestDb");
            Console.WriteLine("\t3 - Fetch Expansions from Web");
            Console.WriteLine("\t4 - Load Expansions from DB");
            Console.WriteLine("\t5 - Save Expansions to Database");
            Console.WriteLine("\t6 - Print expansionList.Length");
            Console.WriteLine("\t7 - null expansionList");
            Console.WriteLine("\tq - quit");


            bool quit = false;
            while (!quit)
            {
                Console.Write("Your option? ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Test();
                        Console.WriteLine(String.Format("Test() done"));
                        break;
                    case "2":
                        TestDb(database);
                        Console.WriteLine(String.Format("TestDb() done"));
                        break;
                    case "3":
                        expansionList = ScrapeExpansionList(scraper);
                        Console.WriteLine(String.Format("{0} Expansions scraped", expansionList.Expansions.Count));
                        break;
                    case "4":
                        expansionList = LoadExpansion(database);
                        Console.WriteLine(String.Format("{0} Expansions loaded", expansionList.Expansions.Count));
                        break;
                    case "5":
                        SaveExpansionList(expansionList, database);
                        Console.WriteLine(String.Format("{0} Expansions saved", expansionList.Expansions.Count));
                        break;
                    case "6":
                        Console.WriteLine(String.Format("{0} Expansions in List", expansionList.Expansions.Count));
                        break;
                    case "7":
                        expansionList = new ExpansionList();
                        Console.WriteLine(String.Format("{0} Expansions in List", expansionList.Expansions.Count));
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

        private static ExpansionList LoadExpansion(ICardmarketData database)
        {
            ExpansionList expansions = database.LoadExpansionList();
            return expansions;
        }

        private static void SaveExpansionList(ExpansionList expansionList, ICardmarketData database)
        {
            database.SaveExpansionList(expansionList);
        }

        private static ExpansionList ScrapeExpansionList(Scraper scraper)
        {
            ExpansionList expansions = scraper.ImportExpansions();
            return expansions;
        }

        private static void TestDb(ICardmarketData database)
        {
            var expansion = new Expansion
            {
                Abbreviation = "FOO",
                EnName = "Foobar",
                IdExpansion = 13,
                ProductCount = 69            
            };

            database.SaveExpansion(expansion);

        }

        private static void Test()
        {
            List<Product> ProdList = new List<Product>();

            ExpansionEntity expent = new ExpansionEntity();
            ;

            Expansion expansion = new Expansion
            {
                EnName = "Erweiterung",
                Products = ProdList
            };
            ExpansionList list = new ExpansionList
            {
                expansion
            };

            Product product = new Product
            {
                EnName = "Produkt",
                Expansion = expansion
            };
            ProdList.Add(product);

            ;
        }
    }
}
