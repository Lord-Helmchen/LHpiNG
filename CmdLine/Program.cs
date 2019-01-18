using LHpiNG.Cardmarket;
using LHpiNG.db;
using LHpiNG.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using SQLite;

namespace LHpiNG
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Program started!");
            TestDb();

            Console.WriteLine("Press any key to quit!");
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
            Sqlite db = new LHpiNG.db.Sqlite();
            db.CreateTables();

        }
        static void Test()
        {
            ExpansionList list = new ExpansionList();

            Expansion expansion = new Expansion();
            expansion.EnName = "Erweiterung";
            list.Add(expansion);

            List<Product> ProdList = new List<Product>();
            expansion.Products = ProdList;

            Product product = new Product();
            product.EnName = "Produkt";
            product.Expansion = expansion;

            ProdList.Add(product);

            ;
        }
    }
}
