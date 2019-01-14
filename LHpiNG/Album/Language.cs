using System;

namespace LHpiNG.Album
{
    class Language
    {
        int Id { get; set; }
        string Abbr { get; set; }
        string M15Abbr { get; set; }
        string Name { get; set; }

        public void LoadFromDatabase()
        {
            throw new NotImplementedException();
        }
        public void SaveToDatabase()
        {
            throw new NotImplementedException();
        }
        public void ImportFromAlbum()
        {
            throw new NotImplementedException();
        }
    }
}