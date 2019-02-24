using LHpiNG.Album;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //AlbumFilePath = @"C:\Users\svc-sp_admin\Source\Repos\LHpiNG\MADatabaseExport";
            AlbumExportFileName = String.Concat(AlbumFilePath, "\\", "AlbumDatabase.csv");
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

        public IEnumerable<AlbumObject> ReadObjects()
        {
            List<AlbumObject> albumObjects = new List<AlbumObject>();
            try
            {
                using (var csv = new CachedCsvReader(new StreamReader(AlbumExportFileName), true, '\t'))
                {
                    csv.MissingFieldAction = MissingFieldAction.ReplaceByNull;
                    string[] headers = csv.GetFieldHeaders();
                    foreach (var row in csv)
                    {
                        AlbumObject albumObject = new AlbumObject
                        {
                            //Set	Name (Oracle)	Version	Language	Rarity	Color	Cost	P/T	Type (Oracle)	Number	
                            //Legality	Artist	Foil	Border	Copyright	Comment	Name	Type	Price (R)	Price (F)	Rating
                            OracleName = row.ElementAt(1),
                            SetTLA = row.ElementAt(0),
                            Version = row.ElementAt(2),
                            LanguageTLA = row.ElementAt(3),
                            RarityString = row.ElementAt(4),
                            Color = row.ElementAt(5),
                            Cost = row.ElementAt(6),
                            PowerToughness = row.ElementAt(7),
                            OracleType = row.ElementAt(8),
                            Number = row.ElementAt(9),
                            Legality = row.ElementAt(10),
                            Artist = row.ElementAt(11),
                            Border = row.ElementAt(13),
                            Copyright = row.ElementAt(14),
                            Comment = row.ElementAt(15),
                            PrintedName = row.ElementAt(16),
                            PrintedType = row.ElementAt(17),
                            PriceRegular = row.ElementAt(18),
                            PriceFoil = row.ElementAt(19),
                            Rating = row.ElementAt(20),
                        };
                        switch (row.ElementAt(12))
                        {
                            case "Yes":
                                albumObject.Foilage |= Album.Foilage.Foil;// SetFlag
                                break;
                            case "No":
                                albumObject.Foilage |= Album.Foilage.Foil;// SetFlag
                                albumObject.Foilage |= Album.Foilage.Nonfoil;// SetFlag
                                break;
                            case "Only":
                                albumObject.Foilage |= Album.Foilage.Nonfoil;//SetFlag
                                break;
                        }
                        switch (row.ElementAt(4))
                        {
                            case "L":
                                albumObject.Rarity = Rarity.BasicLand;
                                break;
                            case "C":
                            case "C1":
                            case "C2":
                            case "C3":
                            case "C4":
                            case "C5":
                            case "CT":// Planar Chaos timeshifted
                                albumObject.Rarity = Rarity.Common;
                                break;
                            case "U":
                            case "U2":
                            case "U3":
                            case "U4":
                            case "UT":// Planar Chaos timeshifted
                                albumObject.Rarity = Rarity.Uncommon;
                                break;
                            case "R":
                            case "U1":
                            case "RT":// Planar Chaos timeshifted
                                albumObject.Rarity = Rarity.Rare;
                                break;
                            case "M":
                                albumObject.Rarity = Rarity.Mythic;
                                break;
                            case "T":
                            case "E":// some Emblems
                            case "O":// fnm doublesided promo tokens
                                albumObject.Rarity = Rarity.Token;
                                break;
                            case "S":// Inventions,Invocations, some Championship Prizes
                                albumObject.Rarity = Rarity.Special;
                                break;
                            case "P":// some Promos
                                albumObject.Rarity = Rarity.Other;
                                break;
                            default:
                                albumObject.Rarity = Rarity.None;
                                break;
                        }
                        string matchedNumber = Regex.Match(albumObject.Number ?? "", @"^T? ?(\d+)").Groups[0].Value;
                        if (int.TryParse(matchedNumber, out int parsedNumber))
                        {
                            albumObject.CollNr = parsedNumber;
                        }
                        if (Enum.TryParse(row.ElementAt(21), true, out ObjectType parsedObjectType))
                        {
                            albumObject.ObjectType = parsedObjectType;
                        }
                        else
                        {
                            throw new FormatException("ObjectType not parsed");
                        }
                        if (albumObject.OracleName == null || albumObject.Version == null || albumObject.SetTLA == null || albumObject.LanguageTLA == null
                            || albumObject.OracleName == string.Empty || albumObject.SetTLA == string.Empty || albumObject.LanguageTLA == string.Empty)
                        {
                            throw new FormatException("Key field not parsed");
                        }
                        albumObjects.Add(albumObject);
                    }
                }
            }
            catch
            {
                throw;
            }
            return albumObjects; // found 244731, which matches MA and csv
        }

        //public void SetDerivedSetAttributes(IEnumerable<Set> sets)
        //{
        //    foreach (Set set in sets)
        //    {
        //        set.CardCount = set.AlbumObjects.Where(o => o.ty)
        //    }
        //}

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
