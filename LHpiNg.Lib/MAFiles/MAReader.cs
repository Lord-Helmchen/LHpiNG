using LHpiNG.Album;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.VisualBasic.FileIO;

namespace LHpiNg.MAFiles
{
    public class MAReader
    {
        private string AlbumFilePath { get; set; }

        public MAReader()
        {
            AlbumFilePath = @"D:\devel\VisualStudioProjects\LHpiNG\MADatabaseExport";
        }

        public IEnumerable<Language> ReadLanguages()
        {
            List<Language> languages = new List<Language>();
            try
            {
                using (var csv = new CachedCsvReader(new StreamReader(AlbumFilePath + @"\Languages.txt"), true, '\t'))
                {
                    foreach (var row in csv)
                    {
                        Language language = new Language
                        {
                            Id = int.Parse(row.ElementAt(0)),
                            Name = row.ElementAt(1)
                        };
                        languages.Add(language);
                    }
                }
            }
            catch
            {
                throw;
            }
            return languages;
        }

        public IEnumerable<Language> CheckLanguageDetails(IEnumerable<Language> languages)
        {
            if (languages.Any(l => l.Abbr == null))
            {
                Console.WriteLine("Languages are missing abbreviations. Please provide:");
            }
            foreach (Language language in languages)
            {
                if (language.Abbr == null || language.Abbr == String.Empty)
                {
                    Console.WriteLine($"Abbreviation for Language {language.Id} ({language.Name})?");
                    string read = Console.ReadLine();
                    if (read != string.Empty)
                    {
                        language.Abbr = read;
                    }
                }
                if (language.M15Abbr == null || language.M15Abbr == String.Empty)
                {
                    Console.WriteLine($"M15 Abbreviation for Language {language.Id} ({language.Name})?");
                    string read = Console.ReadLine();
                    if (read != string.Empty)
                    {
                        language.M15Abbr = read;
                    }
                }
            }
            return languages;
        }


    }
}
