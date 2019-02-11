/// This file is licensed as https://creativecommons.org/licenses/by-sa/3.0/ 
/// source: https://stackoverflow.com/questions/2201595/c-sharp-simplest-way-to-remove-first-occurrence-of-a-substring-from-another-st
/// author: https://stackoverflow.com/users/51090/greg-roberts

namespace LHpiNG.Util
{
    public static class StringExtensions
    {
        public static string Remove(this string source, string remove, int firstN)
        {
            if (firstN <= 0 || string.IsNullOrEmpty(source) || string.IsNullOrEmpty(remove))
            {
                return source;
            }
            int index = source.IndexOf(remove);
            return index < 0 ? source : source.Remove(index, remove.Length).Remove(remove, --firstN);
        }
    }
}