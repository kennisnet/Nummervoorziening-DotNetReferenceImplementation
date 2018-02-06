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
    using System.Collections.Generic;

    /// <summary>
    /// This class reflects the SubmitEckIDBatch operation of the Nummervoorziening service
    /// </summary>
    public class SubmitEckIDBatchOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Submit Eck Id Batch Request object
        /// </summary>
        private readonly SubmitEckIdBatchRequest _submitEckIDBatchRequest = new SubmitEckIdBatchRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly submitEckIdBatchRequest1 _submitEckIDBatchRequestWrapper = new submitEckIdBatchRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitEckIDBatchOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public SubmitEckIDBatchOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Translates the Dictionary to a ListedStampseudonym array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="listedStampseudonym">A list of indexed stampseudonym for which a School ID is requested</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitEckIDBatch(Dictionary<int, string> listedStampseudonym, string chainGuid, string sectorGuid)
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
            _submitEckIDBatchRequest.chainId = chainGuid;
            _submitEckIDBatchRequest.sectorId = sectorGuid;
            _submitEckIDBatchRequest.stampseudonymList = stampseudonymList.ToArray();
            _submitEckIDBatchRequestWrapper.submitEckIdBatchRequest = _submitEckIDBatchRequest;

            // Submit the Request and fetch the Response
            submitEckIdBatchResponse submitEckIDBatchResponseWrapper = _eckIdClient.submitEckIdBatch(_submitEckIDBatchRequestWrapper);

            // Unwrap the Response and return the Batch Identifier
            SubmitBatchResponse submitEckIDBatchResponse = submitEckIDBatchResponseWrapper.submitBatchResponse;

            return submitEckIDBatchResponse.batchIdentifier.Value;
        }
    }
}
