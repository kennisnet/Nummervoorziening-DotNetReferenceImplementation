using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Xml;
using System.Xml.XPath;

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

        /// <summary>
        /// Fetches a Batch based with the given identifier. Incorporates cooldown periods as well as possible Faults.
        /// </summary>
        /// <param name="batchIdentifier">The identifier of the batch to retrieve</param>
        /// <returns>A populated SchoolIDBatch object</returns>
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
                    // Exception is thrown, retrieve the responsible actor to verify the cause
                    switch(GetFaultActorFromException(fe))
                    {
                        // NotFinishedException & TemporaryBlockedException: Wait for the cooling down period to pass, and try again
                        case "NotFinishedException":
                        case "TemporaryBlockedException":
                            break;
                        // ContentAlreadyRetrievedException & ContentRemovedException: No use in trying again, so break the loop                
                        case "ContentAlreadyRetrievedException":                            
                        case "ContentRemovedException":
                        default:
                            throw fe;
                    }
                }
            }

            return schoolIdBatch;
        }

        /// <summary>
        /// Derives the FaultActor from a FaultException
        /// </summary>
        /// <param name="fe">A FaultException</param>
        /// <returns>String containing the Fault Actor</returns>
        private string GetFaultActorFromException(FaultException fe)
        {
            MessageFault fault = fe.CreateMessageFault();
            XmlDocument doc = new XmlDocument();
            XPathNavigator nav = doc.CreateNavigator();

            if (nav != null)
            {
                using (XmlWriter writer = nav.AppendChild())
                {
                    fault.WriteTo(writer, EnvelopeVersion.Soap11);
                }

                XmlNodeList xmlNodeList = doc.GetElementsByTagName("faultactor");

                if (xmlNodeList.Count > 0)
                {
                    XmlNode xmlNode = xmlNodeList.Item(0);
                    return xmlNode.InnerText;
                }
            }

            return "";
        }
    }
}
