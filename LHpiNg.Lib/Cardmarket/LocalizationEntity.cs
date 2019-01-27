using System;
using System.ComponentModel.DataAnnotations;

namespace LHpiNG.Cardmarket
{
    // a name entity per supported language
    public class LocalizationEntity
    {
        public string Name { get; set; }            // the localized name
        public int IdLanguage { get; set; }         // language ID
        public string LanguageName { get; set; }    // language's name in English

        [Key]
        public int Uid { get; set; }                // Entity Framework Primary Key

        public LocalizationEntity()
        {
            
        }
        //constructor from API reply
        public LocalizationEntity(string jsonEntity)
        {
            throw new NotImplementedException();
        }
    }
}