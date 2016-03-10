using CryptSharp.Utility;
using System;
using System.Text;

namespace NVA_DotNetReferenceImplementation.SCrypter
{
    public class ScryptUtil
    {
        public byte[] GenerateHash(string input)
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

        public string GenerateBase64Hash(string input)
        {
            return Convert.ToBase64String(GenerateHash(input));
        }

        public string GenerateHexHash(string input)
        {
            return BitConverter.ToString(GenerateHash(input)).Replace("-", "").ToLower();
        }
    }
}