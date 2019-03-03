using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Util
{
    public static class ArrayExtensions
    {
        // see also https://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net
        public static bool ByteArrayCompare(this byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }
    }
}
