using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RemoveHTML
{
    static class Hasher
    {
        private static byte[] Salt(int length)
        {
            RNGCryptoServiceProvider saltProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[length];
            saltProvider.GetBytes(salt);
            return salt;
        }

        public static string SaltedHash(string word, int saltLength, int hashLength, int NumberOfHashIterations)
        {
            byte[] salt = Salt(saltLength);

            var rfcHasher = new Rfc2898DeriveBytes(word, salt, NumberOfHashIterations);
            byte[] hash = rfcHasher.GetBytes(hashLength);

            byte[] hashWithSalt = new byte[saltLength + hashLength];
            Array.Copy(salt, 0, hashWithSalt, 0, saltLength);
            Array.Copy(hash, 0, hashWithSalt, saltLength, hashLength);

            return Convert.ToBase64String(hashWithSalt);
        }
    }
}
