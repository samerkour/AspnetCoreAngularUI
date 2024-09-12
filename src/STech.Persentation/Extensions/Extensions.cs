using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace STech.Presentation.Api.Extensions
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-7.0
    /// </summary>
    public static class Extensions
    {

        /// <summary>
        /// Generate a 128-bit salt using a sequence of
        /// cryptographically strong random bytes.
        /// </summary>
        /// <returns></returns>
        public static byte[] RandomString()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

            return salt;
        }

        /// <summary>
        /// derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        /// </summary>
        /// <param name="str"> String to be hashed</param>
        /// <param name="salt">Salt to be mixed</param>
        /// <returns></returns>
        public static string HashString(this string str, byte[] salt)
        {

            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (salt.Length == 0)
                return string.Empty;

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: str!,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA256,
             iterationCount: 100000,
             numBytesRequested: 256 / 8));

            return hashed;

        }

    }
}
