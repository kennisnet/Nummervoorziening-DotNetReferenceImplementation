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

namespace NVA_DotNetReferenceImplementation.SchoolID
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Xml;
    using Operations;

    /// <summary>
    /// SchoolIDServiceUtil is a Singleton implementation giving access to several functionalities as presented by the School ID services.
    /// For each functionality, the appropriate Service Operation is invoked to perform the operation. Optionally, the Singleton can be 
    /// used to cache some of the retrieved data (such as the Chains and Sectors), although storage in an external database is preferred.
    /// Be sure to check often if the stored data still matches the live data, as chains and sectors may have been added.
    /// </summary>
    public class SchoolIDServiceUtil
    {
        /// <summary>
        /// Standard Addressing Namespace for SOAP 1.1
        /// </summary>
        private const string WsAddressingNamespace = "http://www.w3.org/2005/08/addressing";

        /// <summary>
        /// Standard anonymous address value for SOAP 1.1 messages
        /// </summary>
        private const string SoapAnonymousOin = "http://www.w3.org/2005/08/addressing/anonymous?oin=";

        /// <summary>
        /// The instance holding the Singleton object of SchoolIDServiceUtil
        /// </summary>
        private static SchoolIDServiceUtil instance;

        /// <summary>
        /// Object to hold retrieved Chains during the session
        /// </summary>
        private static Chain[] chains;

        /// <summary>
        /// Object to hold retrieved Sectors during the session
        /// </summary>
        private static Sector[] sectors;

        /// <summary>
        /// The SOAP proxy class which can directly be used to communicate with the School ID SOAP service
        /// </summary>
        private readonly SchoolIDClient schoolIDClient;
        
        /// <summary>
        /// Holds the path to the certificate as configured in the App.config file
        /// </summary>
        private string pathToCertificate;

        /// <summary>
        /// Holds the decryption password for the certificate as configured in the App.config file
        /// </summary>
        private string certificatePassword;

        /// <summary>
        /// Holds the certificate to use for all communication with the Nummervoorziening service
        /// </summary>
        private X509Certificate2 myCertificate;
        
        /// <summary>
        /// Prevents a default instance of the <see cref="SchoolIDServiceUtil"/> class from being created. 
        /// Creates a new SchoolIDServiceUtil object if instance == null
        /// </summary>
        private SchoolIDServiceUtil()
        {
            this.schoolIDClient = new SchoolIDClient();
        }

        /// <summary>
        /// Gets the Singleton SchoolIDServiceUtil instance
        /// </summary>
        public static SchoolIDServiceUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SchoolIDServiceUtil();
                    instance.InitializeCertificate();
                    instance.SetupAddressingHeaders();
                }

                return instance;
            }
        }
        
        /// <summary>
        /// Checks whether the School ID service is up and running
        /// </summary>
        /// <returns>TRUE if all systems are up</returns>
        public bool IsSchoolIDAvailable()
        {
            PingOperation pingOperation = new PingOperation(this.schoolIDClient);
            return pingOperation.IsAvailable();
        }

        /// <summary>
        /// Retrieves the current DateTime on the School ID server.
        /// </summary>
        /// <returns>DataTime.Now of the School ID server</returns>
        public DateTime? GetSchoolIDDateTime()
        {
            PingOperation pingOperation = new PingOperation(this.schoolIDClient);
            return pingOperation.GetSchoolIDDateTime();
        }

        /// <summary>
        /// Retrieves the current version number of the School ID service.
        /// </summary>
        /// <returns>A string containing the current version number of the School ID service</returns>
        public string GetSchoolIDVersion()
        {
            PingOperation pingOperation = new PingOperation(this.schoolIDClient);
            return pingOperation.GetSchoolIDVersion();
        }

        /// <summary>
        /// Retrieves a list of currently available Chains present in the School ID service.
        /// </summary>
        /// <returns>A Chain[] containing all active chains</returns>
        public Chain[] GetChains()
        {
            if (chains == null || chains.Length == 0)
            {
                RetrieveChainsOperation retrieveChainsOperation = new RetrieveChainsOperation(this.schoolIDClient);
                chains = retrieveChainsOperation.GetChains();
            }

            return chains;
        }

        /// <summary>
        /// Retrieves a list of currently available Sectors present in the School ID service.
        /// </summary>
        /// <returns>A Sector[] containing all active sectors</returns>
        public Sector[] GetSectors()
        {
            if (sectors == null || sectors.Length == 0)
            {
                RetrieveSectorsOperation retrieveSectorsOperation = new RetrieveSectorsOperation(this.schoolIDClient);
                sectors = retrieveSectorsOperation.GetSectors();
            }

            return sectors;
        }

        /// <summary>
        /// Invokes the School ID service to generate a Stampseudonym based on the hashed PGN
        /// </summary>
        /// <param name="hpgn">The scrypt hashed PGN</param>
        /// <returns>If no validation or operational errors, a Stampseudonym</returns>
        public string GenerateStampseudonym(string hpgn)
        {
            RetrieveStampseudonymOperation retrieveStampseudonymOperation = new RetrieveStampseudonymOperation(this.schoolIDClient);
            return retrieveStampseudonymOperation.GetStampseudonym(hpgn);
        }

        /// <summary>
        /// Invokes the School ID service to generate a School ID based on the Stampseudonym, Chain ID and Sector ID
        /// </summary>
        /// <param name="stampseudonym">The scrypt Stampseudonym</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If no validation or operational errors, a School ID</returns>
        public string GenerateSchoolID(string stampseudonym, string chainGuid, string sectorGuid)
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(this.schoolIDClient);
            return retrieveEckIdOperation.GetEckId(stampseudonym, chainGuid, sectorGuid);
        }

        /// <summary>
        /// Invokes the School ID service to substitute the new HPGN with the old HPGN, in order to be able to keep using
        /// the old SchoolID even with a new BSN/PGN
        /// </summary>
        /// <param name="hpgnNew">The scrypt hashed new PGN, which will refer to the old HPGN instead</param>
        /// <param name="hpgnOld">The scrypt hashed old PGN, which the new HPGN will refer to</param>
        /// <param name="effectiveDate">The date the substitution will become active (optional, default value: Now())</param>
        /// <returns>If no validation or operational errors, a School ID based on the active substitution(s)</returns>
        public string ReplaceStampseudonym(string hpgnNew, string hpgnOld, DateTime? effectiveDate)
        {
            ReplaceStampseudonymOperation replaceStampseudonymOperation = new ReplaceStampseudonymOperation(this.schoolIDClient);
            return replaceStampseudonymOperation.ReplaceStampseudonym(hpgnNew, hpgnOld, effectiveDate);
        }

        /// <summary>
        /// Translates the Dictionary to a ListedStampseudonymDictionary array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="stampseudonymDictionary">A list of indexed Stampseudonym for which a School ID is requested</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitEckIdBatch(Dictionary<int, string> stampseudonymDictionary, string chainGuid, string sectorGuid)
        {
            SubmitEckIdBatchOperation submitEckIdBatchOperation = new SubmitEckIdBatchOperation(this.schoolIDClient);
            return submitEckIdBatchOperation.SubmitEckIdBatch(stampseudonymDictionary, chainGuid, sectorGuid);
        }

        /// <summary>
        /// Translates the Dictionary to a ListedHPgnDictionary array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="hpgnDictionary">A list of indexed HPgns for which a Stampseudonym is requested</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitStampseudonymBatch(Dictionary<int, string> hpgnDictionary)
        {
            SubmitStampseudonymBatchOperation submitStampseudonymBatchOperation = new SubmitStampseudonymBatchOperation(this.schoolIDClient);
            return submitStampseudonymBatchOperation.SubmitStampseudonymBatch(hpgnDictionary);
        }

        /// <summary>
        /// Fetches a Batch based with the given identifier. Incorporates cooldown periods as well as possible Faults.
        /// </summary>
        /// <param name="batchIdentifier">The identifier of the batch to retrieve</param>
        /// <returns>A populated SchoolIDBatch object</returns>
        public SchoolIDBatch RetrieveBatch(string batchIdentifier)
        {
            RetrieveBatchOperation retrieveBatchOperation = new RetrieveBatchOperation(this.schoolIDClient);
            return retrieveBatchOperation.RetrieveBatch(batchIdentifier);
        }

        /// <summary>
        /// Function to initialize the certificate used to communicate with the Nummervoorziening service.
        /// </summary>
        private void InitializeCertificate()
        {
            this.pathToCertificate = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["certificateFileName"]);
            this.certificatePassword = ConfigurationManager.AppSettings["certificatePassword"];

            // Explicitly set the configuration to use TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string serviceCertPath = string.Empty;

            // Try to read the certificate, and if successful, add it to the bundle of certificates
            try
            {
                this.myCertificate = new X509Certificate2(this.pathToCertificate, this.certificatePassword);

                serviceCertPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["certificateFileName"]);
                
                EndpointIdentity.CreateDnsIdentity(ConfigurationManager.AppSettings["certificateDnsIdentity"]);

                if (this.schoolIDClient.ClientCredentials != null)
                {
                    this.schoolIDClient.ClientCredentials.ClientCertificate.Certificate = this.myCertificate;
                }
            }
            catch (System.Security.Cryptography.CryptographicException crex)
            {
                System.Diagnostics.Debug.WriteLine("Cryptographic Exc: " + crex.Message + " - " + this.pathToCertificate + " - " + serviceCertPath);
            }
        }

        /// <summary>
        /// Sets up the standard SOAP addressing headers (which are missing from the default configuration in WCF)
        /// </summary>
        private void SetupAddressingHeaders()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "Address", WsAddressingNamespace);
            xmlNode.InnerText = SoapAnonymousOin + ConfigurationManager.AppSettings["instanceOIN"];

            AddressHeader fromHeader = AddressHeader.CreateAddressHeader("From", WsAddressingNamespace, xmlNode);

            EndpointAddressBuilder endpointAddressBuilder = new EndpointAddressBuilder(this.schoolIDClient.Endpoint.Address);
            endpointAddressBuilder.Headers.Add(fromHeader);

            this.schoolIDClient.Endpoint.Address = endpointAddressBuilder.ToEndpointAddress();
        }
    }
}