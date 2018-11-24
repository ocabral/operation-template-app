using System;
using System.Security.Cryptography;
using System.Text;

namespace StoneCo.Buy4.OperationTemplate.Core.Operations.Authentication
{
    /// <summary>
    /// Cryptography Extensions.
    /// </summary>
    public static class CryptographyExtensions
    {
        /// <summary>
        /// Encrypts the specified private key to HMACSHA256 string.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetHMACSHA256(this string privateKey, string value)
        {
            using (HMACSHA256 encrypt = new HMACSHA256(Encoding.UTF8.GetBytes(privateKey)))
            {
                byte[] signature = Encoding.UTF8.GetBytes(value);
                byte[] signatureBytes = encrypt.ComputeHash(signature);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in signatureBytes)
                {
                    hashString.AppendFormat("{0:x2}", b);
                }

                return hashString.ToString();
            }
        }

        /// <summary>
        /// Encrypts to HMACSHA256 byte array.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] GetHMACSHA256ByteArray(this string privateKey, string value)
        {
            using (HMACSHA256 encrypt = new HMACSHA256(Encoding.UTF8.GetBytes(privateKey)))
            {
                byte[] signature = Encoding.UTF8.GetBytes(value);
                return encrypt.ComputeHash(signature);
            }
        }

        /// <summary>
        /// Calculates the MD5 hash for the given string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>A 32 char long hash.</returns>
        public static string GetMD5(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            byte[] hashBytes = hasher.ComputeHash(inputBytes);
            StringBuilder hash = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                hash.AppendFormat("{0:x2}", b);
            }

            return hash.ToString();
        }

        /// <summary>
        /// Calculates the SHA256 hash for the given string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns>A SHA256 char long hash.</returns>
        public static string GetSHA256(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            byte[] data = Encoding.UTF8.GetBytes(input);
            using (SHA256Managed sha = new SHA256Managed())
            {
                sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        /// <summary>
        /// Generates a random client token.
        /// </summary>
        public static string GenerateRandomClientToken()
        {
            string finalString = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                finalString += Guid.NewGuid().ToString("N");
            }

            Random rand = new Random();
            int randomNumber = rand.Next(64, 128);

            return finalString.Substring(0, randomNumber);
        }
    }
}
