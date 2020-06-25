using System;
using System.Security.Cryptography;

namespace WordCounter.Services
{
    /// <summary>
    /// Creates salted hash for specified word
    /// </summary>
    public static class SaltHash
    {
        /// <summary>
        /// Creates a salt with specified length
        /// </summary>
        /// <param name="length">length of salt in bytes</param>
        /// <returns></returns>
        private static byte[] Salt(int length)
        {
            RNGCryptoServiceProvider saltProvider = new RNGCryptoServiceProvider();
            
            byte[] salt = new byte[length];

            // write random bytes to salt array
            saltProvider.GetBytes(salt);
            
            return salt;
        }

        /// <summary>
        /// Creates salted hash
        /// </summary>
        /// <param name="word">a string which is used for hash</param>
        /// <param name="saltLength">desired salt length</param>
        /// <param name="hashLength"></param>
        /// <param name="NumberOfHashIterations"></param>
        /// <returns></returns>
        public static string GetSaltedHash(string word, int saltLength, int hashLength, int NumberOfHashIterations)
        {
            // check values (limitations are set according to official documentation (NSDN))
            if (saltLength < 8) return string.Empty;
            if (hashLength < 1) return string.Empty;
            if (NumberOfHashIterations < 1) NumberOfHashIterations = 1000;

            byte[] salt = Salt(saltLength);

            // get hash
            var rfcHasher = new Rfc2898DeriveBytes(word, salt, NumberOfHashIterations);
            byte[] hash = rfcHasher.GetBytes(hashLength);

            // combine salt and hash
            byte[] hashWithSalt = new byte[saltLength + hashLength];
            Array.Copy(salt, 0, hashWithSalt, 0, saltLength);
            Array.Copy(hash, 0, hashWithSalt, saltLength, hashLength);

            // get string represenation of salted hash
            return Convert.ToBase64String(hashWithSalt);
        }
    }
}
