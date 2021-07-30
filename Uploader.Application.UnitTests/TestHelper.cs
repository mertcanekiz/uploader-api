using System;
using System.Security.Cryptography;

namespace Uploader.Application.UnitTests
{
    public static class TestHelper
    {
        public static string GetUniqueString(int length)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bitCount = (length * 6);
            var byteCount = ((bitCount + 7) / 8);
            var bytes = new byte[byteCount];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}