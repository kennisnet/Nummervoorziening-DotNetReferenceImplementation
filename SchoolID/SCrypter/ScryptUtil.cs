using CryptSharp.Utility;
using System;
using System.Text;

namespace NVA_DotNetReferenceImplementation.SCrypter
{
    public class ScryptUtil
    {
        /// <summary>
        /// Returns a scrypted hash as a Byte array
        /// </summary>
        /// <param name="input">The input to hash</param>
        /// <returns>A Byte array</returns>
        private byte[] GenerateHash(string input)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(input.ToLower());
            byte[] saltBytes = Convert.FromBase64String(Constants.SALT);

            return SCrypt.ComputeDerivedKey(
                keyBytes, 
                saltBytes, 
                Constants.N,
                Constants.r,
                Constants.p,
                Constants.MAX_THREADS, 
                Constants.DERIVED_KEY_LENGTH);
        }
        
        /// <summary>
        /// Returns a scrypted hash in hexadecimal notation. For the sake of standardization and to prevent 
        /// mismatches, the hexadecimal String is lower cased.
        /// </summary>
        /// <param name="input">The input to hash</param>
        /// <returns>A lowercased hexadecimal String</returns>
        public string GenerateHexHash(string input)
        {
            return BitConverter.ToString(GenerateHash(input)).Replace("-", "").ToLower();
        }
    }
}