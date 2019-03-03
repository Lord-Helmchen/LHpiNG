using System.Collections.Generic;

namespace LHpiNG.Album
{
    public interface IFromAlbum
    {
        IEnumerable<Card> ReadCards();
        IEnumerable<Language> ReadLanguages();
        IEnumerable<Set> ReadSets();
    }
}