using LHpiNG.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LHpiNG.db
{
    public interface IAlbumData
    {
        //declare getters and setters as part of the interface to force me to remember to implement the fields
        DbSet<AlbumObject> AlbumObjects { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Set> Sets { get; set; }

        IEnumerable<Language> LoadLanguages();
        IEnumerable<AlbumObject> LoadObjects();
        IEnumerable<Set> LoadSets();
        void SaveAlbumObjects(IEnumerable<AlbumObject> albumObjects);
        void SaveLanguages(IEnumerable<Language> languages);
        void SaveSets(IEnumerable<Set> sets);

    }
}