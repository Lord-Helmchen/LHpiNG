/// source: https://gist.github.com/Tibo-lg/4ac1234995343335a0813cb29405687c

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public static class Sha256Helper
{
    [ThreadStatic]
    private static SHA256 _sha256;

    private static SHA256 Sha256
    {
        get
        {
            if (_sha256 is null)
            {
                _sha256 = SHA256.Create();
            }

            return _sha256;
        }
    }

    public static string GenerateHash(string input)
    {
        var bytes = Encoding.Unicode.GetBytes(input);
        var hash = HexStringFromBytes(Sha256.ComputeHash(bytes, 0, bytes.Length));

        return hash;
    }

    public static byte[] GenerateHashBytes(string input)
    {
        var bytes = Encoding.Unicode.GetBytes(input);
        var hashBytes = Sha256.ComputeHash(bytes, 0, bytes.Length);

        return hashBytes;
    }

    public static string HexStringFromBytes(IEnumerable<byte> bytes)
    {
        var sb = new StringBuilder();

        foreach (var b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}