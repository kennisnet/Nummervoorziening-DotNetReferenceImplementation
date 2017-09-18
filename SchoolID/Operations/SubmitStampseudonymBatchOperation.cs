using System.Collections.Generic;

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    public class SubmitStampseudonymBatchOperation
    {
        /// <summary>
        /// The SchoolID object for communication with the service
        /// </summary>
        private SchoolIDClient schoolIDClient;

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly submitStampseudonymBatchRequest submitStampseudonymBatchRequestWrapper = new submitStampseudonymBatchRequest();

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitStampseudonymBatchOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public SubmitStampseudonymBatchOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
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
            this.submitStampseudonymBatchRequestWrapper.submitStampseudonymBatchRequest1 = hpgnList.ToArray();

            // Submit the Request and fetch the Response
            submitEckIdBatchResponse submitEckIdBatchResponseWrapper = this.schoolIDClient.submitStampseudonymBatch(this.submitStampseudonymBatchRequestWrapper);

            // Unwrap the Response and return the Batch Identifier
            SubmitBatchResponse submitEckIdBatchResponse = submitEckIdBatchResponseWrapper.submitBatchResponse;

            return submitEckIdBatchResponse.batchIdentifier.Value;
        }
    }
}