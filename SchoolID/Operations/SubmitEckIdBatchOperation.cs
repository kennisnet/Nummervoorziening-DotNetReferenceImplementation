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

using System.Collections.Generic;

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    class SubmitEckIdBatchOperation
    {
        private SchoolIDClient schoolIDClient;
        private SubmitEckIdBatchRequest submitEckIdBatchRequest = new SubmitEckIdBatchRequest();
        private submitEckIdBatchRequest1 submitEckIdBatchRequestWrapper = new submitEckIdBatchRequest1();

        /// <summary>
        /// Sets up the RetrieveSectorsOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public SubmitEckIdBatchOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Translates the Dictionary to a ListedHpgn array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="listedHpgn">A list of indexed Hpgn for which a School ID is requested</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitHpgnBatch(Dictionary<int, string> listedHpgn, string chainGuid, string sectorGuid)
        {
            List<ListedHpgn> hpgnList = new List<ListedHpgn>();                        
            
            // Loop through the Dictionary to convert its contents to a ListedHpgn array
            foreach(KeyValuePair<int, string> entry in listedHpgn)
            {
                ListedHpgn currListedHpgn = new ListedHpgn();
                HPgn currHpgn = new HPgn();
                
                currListedHpgn.index = entry.Key;
                currHpgn.Value = entry.Value;
                currListedHpgn.hPgn = currHpgn;
                hpgnList.Add(currListedHpgn);
            }

            // Create the Request
            submitEckIdBatchRequest.chainId = chainGuid;
            submitEckIdBatchRequest.sectorId = sectorGuid;
            submitEckIdBatchRequest.hpgnList = hpgnList.ToArray();
            submitEckIdBatchRequestWrapper.submitEckIdBatchRequest = submitEckIdBatchRequest;

            // Submit the Request and fetch the Response
            submitEckIdBatchResponse1 submitEckIdBatchResponseWrapper = schoolIDClient.submitEckIdBatch(submitEckIdBatchRequestWrapper);

            // Unwrap the Response and return the Batch Identifier
            SubmitEckIdBatchResponse submitEckIdBatchResponse = submitEckIdBatchResponseWrapper.submitEckIdBatchResponse;

            return submitEckIdBatchResponse.batchIdentifier.Value;
        }
    }
}
