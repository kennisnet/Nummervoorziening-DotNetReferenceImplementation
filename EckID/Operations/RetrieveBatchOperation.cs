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
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading;
    using System.Xml;
    using System.Xml.XPath;

    /// <summary>
    /// This class reflects the Retrieve Batch operation of the Nummervoorziening service
    /// </summary>
    public class RetrieveBatchOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Retrieve Batch Request object
        /// </summary>
        private readonly RetrieveBatchRequest _retrieveBatchRequest = new RetrieveBatchRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveBatchRequest1 _retrieveBatchRequestWrapper = new retrieveBatchRequest1();

        /// <summary>
        /// Amount of times to try to retrieve a batch before failing
        /// </summary>
        private const int BatchRetrieveAttemptsCount = 10;

        /// <summary>
        /// Amount of time in milliseconds to wait before retrying to fetch a batch
        /// </summary>
        private const int RetrieveSchoolIdBatchTimeout = 25000;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveBatchOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public RetrieveBatchOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Fetches a Batch based with the given identifier. Incorporates cooldown periods as well as possible Faults.
        /// </summary>
        /// <param name="batchIdentifier">The identifier of the batch to retrieve</param>
        /// <returns>A populated EckIDBatch object</returns>
        public EckIDBatch RetrieveBatch(string batchIdentifier)
        {
            EckIDBatch eckIdBatch = new EckIDBatch();
            _retrieveBatchRequest.batchIdentifier = new BatchIdentifier {Value = batchIdentifier};
            _retrieveBatchRequestWrapper.retrieveBatchRequest = _retrieveBatchRequest;
            
            // Try to retrieve the Batch, retry if it is not ready yet (a FaultException will be thrown)
            for (int i = 0; i < BatchRetrieveAttemptsCount; i++)
            {
                Thread.Sleep(RetrieveSchoolIdBatchTimeout);

                try
                {
                    retrieveBatchResponse1 retrieveBatchResponseWrapper =
                        _eckIdClient.retrieveBatch(_retrieveBatchRequestWrapper);

                    RetrieveBatchResponse retrieveBatchResponse =
                        retrieveBatchResponseWrapper.retrieveBatchResponse;

                    ListedEntitySuccess[] successListed = retrieveBatchResponse.success;
                    ListedEntityFailure[] failureListed = retrieveBatchResponse.failed;

                    eckIdBatch.SetSuccessList(retrieveBatchResponse.success);
                    eckIdBatch.SetFailedList(retrieveBatchResponse.failed);

                    break;
                }
                catch (FaultException fe)
                {
                    // Exception is thrown, retrieve the responsible actor to verify the cause
                    switch (GetFaultActorFromException(fe))
                    {
                        // NotFinishedException & TemporaryBlockedException: Wait for the cooling down period to pass, and try again
                        case "NotFinishedException":
                        case "TemporaryBlockedException":
                            break;

                        // ContentAlreadyRetrievedException & ContentRemovedException: No use in trying again, so break the loop                
                        case "ContentAlreadyRetrievedException":                            
                        case "ContentRemovedException":
                        default:
                            throw;
                    }
                }
            }

            return eckIdBatch;
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

            return string.Empty;
        }
    }
}
