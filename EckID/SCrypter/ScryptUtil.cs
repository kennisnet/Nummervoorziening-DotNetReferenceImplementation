#region License
/*
Copyright 2016, Stichting Kennisnet

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System;
using System.Text;
using CryptSharp.Utility;

namespace EckID.SCrypter
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