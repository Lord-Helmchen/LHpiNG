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
        private string AlbumExportFileName { get; set; }

        public MAReader()
        {
            AlbumFilePath = @"D:\devel\VisualStudioProjects\LHpiNG\MADatabaseExport";
            AlbumExportFileName = String.Concat(AlbumFilePath, "AlbumDatabase.csv");
        }

        public IEnumerable<Language> ReadLanguages()
        {
            List<Language> languages = new List<Language>();
            try
            {
                using (var csv = new CachedCsvReader(new StreamReader(AlbumFilePath + @"\Languages.txt"), false, '\t'))
                {
                    foreach (var row in csv)
                    {
                        Language language = new Language
                        {
                            Id = int.Parse(row.ElementAt(0)),
                            TLA = row.ElementAt(1),
                            Name = row.ElementAt(2)
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
        
        public IEnumerable<Set> ReadSets()
        {
            List<Set> sets = new List<Set>();
            try
            {
                using (var csv = new CachedCsvReader(new StreamReader(AlbumFilePath + @"\Sets.txt"), false, '\t'))
                {
                    foreach (var row in csv)
                    {
                        Set set = new Set
                        {
                            Id = int.Parse(row.ElementAt(0)),
                            TLA = row.ElementAt(1),
                            Name = row.ElementAt(2)
                        };
                        sets.Add(set);
                    }
                }
            }
            catch
            {
                throw;
            }
            return sets;
        }



        [Obsolete]//due to Goblin Hero gracefully adding TLAs to Languages.txt
        public IEnumerable<Language> CheckLanguageDetails(IEnumerable<Language> languages)
        {
            if (languages.Any(l => l.TLA == null))
            {
                Console.WriteLine("Languages are missing abbreviations. Please provide:");
            }
            foreach (Language language in languages)
            {
                if (language.TLA == null || language.TLA == String.Empty)
                {
                    Console.WriteLine($"Abbreviation for Language {language.Id} ({language.Name})?");
                    string read = Console.ReadLine();
                    if (read != string.Empty)
                    {
                        language.TLA = read;
                    }
                }
                if (language.M15Abbr == null || language.M15Abbr == String.Empty)
                {
                    Console.WriteLine($"M15 Abbreviation for Language {language.Id} ({language.Name}) (just press ENTER if not applicable)?");
                    string read = Console.ReadLine();
                    if (read != string.Empty)
                    {
                        language.M15Abbr = read;
                    }
                }
            }
            return languages;
        }
        [Obsolete]//due to Goblin Hero gracefully adding TLAs to Sets.txt
        public IEnumerable<Set> CheckSetDetails(IEnumerable<Set> sets)
        {
            if (sets.Any(l => l.TLA == null))
            {
                Console.WriteLine("Languages are missing abbreviations. Please provide:");
            }
            foreach (Set set in sets)
            {
                if (set.TLA == null || set.TLA == String.Empty)
                {
                    Console.WriteLine($"Abbreviation for Set {set.Id} ({set.Name})?");
                    string read = Console.ReadLine();
                    if (read != string.Empty)
                    {
                        set.TLA = read;
                    }
                }
            }
            return sets;
        }

    }
}
