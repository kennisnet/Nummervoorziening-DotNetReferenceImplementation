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

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    using System.Collections.Generic;

    /// <summary>
    /// This class reflects the SubmitEckIdBatch operation of the Nummervoorziening service
    /// </summary>
    public class SubmitEckIdBatchOperation
    {
        /// <summary>
        /// The SchoolID object for communication with the service
        /// </summary>
        private SchoolIDClient schoolIDClient;

        /// <summary>
        /// The actual Submit Eck Id Batch Request object
        /// </summary>
        private readonly SubmitEckIdBatchRequest submitEckIdBatchRequest = new SubmitEckIdBatchRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly submitEckIdBatchRequest1 submitEckIdBatchRequestWrapper = new submitEckIdBatchRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitEckIdBatchOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public SubmitEckIdBatchOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Translates the Dictionary to a ListedStampseudonym array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="listedStampseudonym">A list of indexed stampseudonym for which a School ID is requested</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitEckIdBatch(Dictionary<int, string> listedStampseudonym, string chainGuid, string sectorGuid)
        {
            List<ListedStampseudonym> stampseudonymList = new List<ListedStampseudonym>();

            // Loop through the Dictionary to convert its contents to a ListedStampseudonym array
            foreach (KeyValuePair<int, string> entry in listedStampseudonym)
            {
                ListedStampseudonym currListedStampseudonym = new ListedStampseudonym();
                Stampseudonym currStampseudonym = new Stampseudonym();
                
                currListedStampseudonym.index = entry.Key;
                currStampseudonym.Value = entry.Value;
                currListedStampseudonym.stampseudonym = currStampseudonym;
                stampseudonymList.Add(currListedStampseudonym);
            }

            // Create the Request
            this.submitEckIdBatchRequest.chainId = chainGuid;
            this.submitEckIdBatchRequest.sectorId = sectorGuid;
            this.submitEckIdBatchRequest.stampseudonymList = stampseudonymList.ToArray();
            this.submitEckIdBatchRequestWrapper.submitEckIdBatchRequest = this.submitEckIdBatchRequest;

            // Submit the Request and fetch the Response
            submitEckIdBatchResponse submitEckIdBatchResponseWrapper = this.schoolIDClient.submitEckIdBatch(this.submitEckIdBatchRequestWrapper);

            // Unwrap the Response and return the Batch Identifier
            SubmitBatchResponse submitEckIdBatchResponse = submitEckIdBatchResponseWrapper.submitBatchResponse;

            return submitEckIdBatchResponse.batchIdentifier.Value;
        }
    }
}
