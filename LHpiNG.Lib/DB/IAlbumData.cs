using LHpiNG.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LHpiNG.DB
{
    public interface IAlbumData
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        DbSet<Card> Cards { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Set> Sets { get; set; }

        IEnumerable<Language> LoadLanguages();
        IEnumerable<Card> LoadCards();
        IEnumerable<Set> LoadSets();
        void SaveCards(IEnumerable<Card> cards);
        void SaveLanguages(IEnumerable<Language> languages);
        void SaveSets(IEnumerable<Set> sets);

    }
}