using System.Collections.Generic;

namespace EckID.Operations
{
    public class SubmitStampseudonymBatchOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private EckIDPortClient _eckIdClient;

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly submitStampseudonymBatchRequest _submitStampseudonymBatchRequestWrapper = new submitStampseudonymBatchRequest();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitStampseudonymBatchOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public SubmitStampseudonymBatchOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Translates the Dictionary to a ListedHPgn array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="listedHPgn">A list of indexed HPgns for which a School ID is requested</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitStampseudonymBatch(Dictionary<int, string> listedHPgn)
        {
            List<ListedHpgn> hpgnList = new List<ListedHpgn>();

            // Loop through the Dictionary to convert its contents to a ListedStampseudonym array
            foreach (KeyValuePair<int, string> entry in listedHPgn)
            {
                ListedHpgn currListedHPgn = new ListedHpgn();
                HPgn currHPgn = new HPgn();

                currListedHPgn.index = entry.Key;
                currHPgn.Value = entry.Value;
                currListedHPgn.hPgn = currHPgn;
                hpgnList.Add(currListedHPgn);
            }

            // Create the Request
            _submitStampseudonymBatchRequestWrapper.submitStampseudonymBatchRequest1 = hpgnList.ToArray();

            // Submit the Request and fetch the Response
            submitEckIdBatchResponse submitEckIdBatchResponseWrapper = 
                _eckIdClient.submitStampseudonymBatch(_submitStampseudonymBatchRequestWrapper);

            // Unwrap the Response and return the Batch Identifier
            SubmitBatchResponse submitEckIdBatchResponse = submitEckIdBatchResponseWrapper.submitBatchResponse;

            return submitEckIdBatchResponse.batchIdentifier.Value;
        }
    }
}