using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    class RetrieveEckIdBatchOperation
    {
        private SchoolIDClient schoolIDClient;
        private RetrieveEckIdBatchRequest retrieveEckIdBatchRequest = new RetrieveEckIdBatchRequest();
        private retrieveEckIdBatchRequest1 retrieveEckIdBatchRequestWrapper = new retrieveEckIdBatchRequest1();
        
        private int BATCH_RETRIEVE_ATTEMPTS_COUNT = 10;
        private int RETRIEVE_SCHOOL_ID_BATCH_TIMEOUT = 25000;        

        /// <summary>
        /// Sets up the RetrieveSectorsOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveEckIdBatchOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }
        
        public SchoolIDBatch RetrieveBatch(string batchIdentifier)
        {
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();
            retrieveEckIdBatchRequest.batchIdentifier = new BatchIdentifier();            
            retrieveEckIdBatchRequest.batchIdentifier.Value = batchIdentifier;
            retrieveEckIdBatchRequestWrapper.retrieveEckIdBatchRequest = retrieveEckIdBatchRequest;
            
            // Try to retrieve the Batch, retry if it is not ready yet (a FaultException will be thrown)
            for (int i = 0; i < BATCH_RETRIEVE_ATTEMPTS_COUNT; i++)
            {
                Thread.Sleep(RETRIEVE_SCHOOL_ID_BATCH_TIMEOUT);

                try
                {
                    retrieveEckIdBatchResponse1 retrieveEckIdBatchResponseWrapper =
                        schoolIDClient.retrieveEckIdBatch(retrieveEckIdBatchRequestWrapper);

                    RetrieveEckIdBatchResponse retrieveEckIdBatchResponse = 
                        retrieveEckIdBatchResponseWrapper.retrieveEckIdBatchResponse;                    

                    ListedEckIdSuccess[] successListedEckId = retrieveEckIdBatchResponse.success;
                    ListedEckIdFailure[] failureListedEckId = retrieveEckIdBatchResponse.failed;

                    schoolIdBatch.setSuccessList(retrieveEckIdBatchResponse.success);
                    schoolIdBatch.setFailedList(retrieveEckIdBatchResponse.failed);

                    break;
                }
                catch (FaultException fe)
                {
                    // Exception is thrown, wait for the Cooldown period to pass
                }
            }

            return schoolIdBatch;
        }           
    }
}
