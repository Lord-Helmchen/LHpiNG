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
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Program started!");
            Console.WriteLine("Choose which method to run:");
            Console.WriteLine("\t1 - Test()");
            Console.WriteLine("\t2 - TestScraper()");
            Console.WriteLine("\t3 - TestDb");
            Console.WriteLine("\tq - quit");
            Console.Write("Your option? ");

            switch (Console.ReadLine())
            {
                case "1":
                    Test();
                    break;
                case "2":
                    TestScrape();
                    break;
                case "3":
                    TestDb();
                    break;
                default:
                    break;
            }

            Console.WriteLine("Press any key to close the window!");
            Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
        static void TestScrape()
        {
            Scraper scraper = new Scraper();
            ExpansionList expansions = scraper.ImportExpansions();

        }
        static void TestDb()
        {
            var expansion = new Expansion
            {
                Abbreviation = "FOO",
                EnName = "Foobar",
                IdExpansion = 13
            };

            //using (var ctx = new LHpiNG.db.SQLContext())
            using (var ctx = new LHpiNG.db.SQLiteContext())
            {


                ctx.Expansions.Add(expansion);
                ctx.SaveChanges();

            }
        }
        static void Test()
        {
            List<Product> ProdList = new List<Product>();

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
