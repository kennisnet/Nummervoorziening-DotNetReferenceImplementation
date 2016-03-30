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
