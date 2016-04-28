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

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    /// <summary>
    /// This class reflects the ReplaceEckId operation of the Nummervoorziening service
    /// </summary>
    class ReplaceEckIdOperation
    {
        private SchoolIDClient schoolIDClient;

        /// <summary>
        /// Sets up the RetrieveChainsOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public ReplaceEckIdOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Provides the parameters as a ReplaceEckIdRequest to the Nummervoorziening service, fetches the ReplaceEckIdResponse and returns the ECK ID.
        /// </summary>
        /// <param name="newHpgnValue">The scrypt hashed new PGN</param>
        /// <param name="oldHpgnValue">The scrypt hashed old PGN</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <param name="effectiveDate">The date for the substitution to become active (optional)</param>
        /// <returns>The generated School ID</returns>
        public string ReplaceEckId(string newHpgnValue, string oldHpgnValue, string chainGuid, string sectorGuid, DateTime? effectiveDate)
        {
            ReplaceEckIdRequest replaceEckIdRequest = new ReplaceEckIdRequest();
            HPgn newHpgn = new HPgn();
            HPgn oldHpgn = new HPgn();

            newHpgn.Value = newHpgnValue;
            oldHpgn.Value = oldHpgnValue;

            replaceEckIdRequest.hpgnNew = newHpgn;
            replaceEckIdRequest.hpgnOld = oldHpgn;
            replaceEckIdRequest.chainId = chainGuid;
            replaceEckIdRequest.sectorId = sectorGuid;

            if (effectiveDate != null)
            {
                replaceEckIdRequest.effectiveDateSpecified = true;
                replaceEckIdRequest.effectiveDate = (DateTime)effectiveDate;
            }

            replaceEckIdRequest1 replaceEckIdRequestWrapper = new replaceEckIdRequest1();
            replaceEckIdRequestWrapper.replaceEckIdRequest = replaceEckIdRequest;
            replaceEckIdResponse1 replaceEckIdResponseWrapper = schoolIDClient.replaceEckId(replaceEckIdRequestWrapper);
            ReplaceEckIdResponse replaceEckIdResponse = replaceEckIdResponseWrapper.replaceEckIdResponse;
                        
            EckId eckId = replaceEckIdResponse.eckId;

            return eckId.Value;
        }
    }
}
