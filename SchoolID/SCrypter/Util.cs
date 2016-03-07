using CryptSharp.Utility;
using System;
using System.Text;

namespace NVA_DotNetReferenceImplementation.SCrypter
{
    public class Util
    {
        public byte[] GenerateHash(string input)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(input);
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
    }
}