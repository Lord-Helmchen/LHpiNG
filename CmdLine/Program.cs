using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LHpiNG.Album;
using LHpiNG.Cardmarket;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace LHpiNG
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Program started!");
            Test();

            Console.WriteLine("Press any key to quit!");
            Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
        static void Test()
        {
            Scraper scraper = new Scraper();
            ExpansionList expansions = new ExpansionList();
            expansions.ImportFromWeb(scraper);



        }

    }
}
