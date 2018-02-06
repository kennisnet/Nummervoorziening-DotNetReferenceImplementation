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

namespace EckID.Operations
{
    using System;

    /// <summary>
    /// This class reflects the ReplaceStampseudonym operation of the Nummervoorziening service
    /// </summary>
    public class ReplaceStampseudonymOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private EckIDPortClient _eckIdClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceStampseudonymOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public ReplaceStampseudonymOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Provides the parameters as a ReplaceEckIDRequest to the Nummervoorziening service, fetches the ReplaceEckIDResponse and returns the ECK ID.
        /// </summary>
        /// <param name="newHpgnValue">The scrypt hashed new PGN</param>
        /// <param name="oldHpgnValue">The scrypt hashed old PGN</param>
        /// <param name="effectiveDate">The date for the substitution to become active (optional)</param>
        /// <returns>The generated School ID</returns>
        public string ReplaceStampseudonym(string newHpgnValue, string oldHpgnValue, DateTime? effectiveDate)
        {
            ReplaceStampseudonymRequest replaceEckIDRequest = new ReplaceStampseudonymRequest();
            HPgn newHpgn = new HPgn();
            HPgn oldHpgn = new HPgn();

            newHpgn.Value = newHpgnValue;
            oldHpgn.Value = oldHpgnValue;

            replaceEckIDRequest.hpgnNew = newHpgn;
            replaceEckIDRequest.hpgnOld = oldHpgn;

            if (effectiveDate != null)
            {
                replaceEckIDRequest.effectiveDateSpecified = true;
                replaceEckIDRequest.effectiveDate = (DateTime)effectiveDate;
            }

            replaceStampseudonymRequest1 replaceEckIDRequestWrapper = new replaceStampseudonymRequest1();
            replaceEckIDRequestWrapper.replaceStampseudonymRequest = replaceEckIDRequest;
            replaceStampseudonymResponse1 replaceEckIDResponseWrapper = _eckIdClient.replaceStampseudonym(replaceEckIDRequestWrapper);
            ReplaceStampseudonymResponse replaceEckIDResponse = replaceEckIDResponseWrapper.replaceStampseudonymResponse;
                        
            Stampseudonym eckId = replaceEckIDResponse.stampseudonym;

            return eckId.Value;
        }
    }
}
