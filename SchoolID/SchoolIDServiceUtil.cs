using NVA_DotNetReferenceImplementation.SchoolID.Operations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace NVA_DotNetReferenceImplementation.SchoolID
{
    /// <summary>
    /// SchoolIDServiceUtil is a Singleton implementation giving access to several functionalities as presented by the School ID services.
    /// For each functionality, the appropriate Service Operation is invoked to perform the operation. Optionally, the Singleton can be 
    /// used to cache some of the retrieved data (such as the Chains and Sectors), although storage in an external database is preferred.
    /// Be sure to check often if the stored data still matches the live data, as chains and sectors may have been added.
    /// </summary>
    public class SchoolIDServiceUtil
    {
        /// <summary>
        /// The SOAP proxy class which can directly be used to communicate with the School ID SOAP service
        /// </summary>
        private SchoolIDClient schoolIDClient;

        /// <summary>
        /// The instance holding the Singleton object of SchoolIDServiceUtil
        /// </summary>
        private static SchoolIDServiceUtil instance;
        
        private string PathToCertificate;

        private string CertificatePassword;

        private X509Certificate2 MyCertificate;
        
        private X509Certificate2 ServiceCertificate;

        private EndpointIdentity MyIdentity;

        /// <summary>
        /// Object to hold retrieved Chains during the session
        /// </summary>
        private static Chain[] chains;

        /// <summary>
        /// Object to hold retrieved Sectors during the session
        /// </summary>
        private static Sector[] sectors;

        /// <summary>
        /// Creates a new SchoolIDServiceUtil object if instance == null
        /// </summary>
        private SchoolIDServiceUtil() {
            schoolIDClient = new SchoolIDClient();
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
                }
                return instance;
            }
        }

        private void InitializeCertificate()
        {
            PathToCertificate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                ConfigurationManager.AppSettings["certificateFileName"]);
            CertificatePassword = ConfigurationManager.AppSettings["certificatePassword"];

            string serviceCertPath = "";

            // Try to read the certificate, and if successful, add it to the bundle of certificates
            try
            {
                MyCertificate = new X509Certificate2(PathToCertificate, CertificatePassword);

                serviceCertPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["certificateFileName"]);

                ServiceCertificate = new X509Certificate2(serviceCertPath,
                    ConfigurationManager.AppSettings["certificatePassword"]);
                
                MyIdentity = EndpointIdentity.CreateDnsIdentity(
                    ConfigurationManager.AppSettings["certificateDnsIdentity"]);

                string oin = ConfigurationManager.AppSettings["instanceOIN"];
                
                schoolIDClient.ClientCredentials.ClientCertificate.Certificate = MyCertificate;
            }
            catch (System.Security.Cryptography.CryptographicException crex)
            {
                System.Diagnostics.Debug.WriteLine("Cryptographic Exc: " + crex.Message + " - " + PathToCertificate + " - " + serviceCertPath);
            }
        }

        /// <summary>
        /// Checks whether the School ID service is up and running
        /// </summary>
        /// <returns>TRUE if all systems are up</returns>
        public bool IsSchoolIDAvailable()
        {
            PingOperation pingOperation = new PingOperation(schoolIDClient);
            return pingOperation.IsAvailable();
        }

        /// <summary>
        /// Retrieves the current DateTime on the School ID server.
        /// </summary>
        /// <returns>DataTime.Now of the School ID server</returns>
        public DateTime? GetSchoolIDDateTime()
        {
            PingOperation pingOperation = new PingOperation(schoolIDClient);
            return pingOperation.GetSchoolIDDateTime();
        }

        /// <summary>
        /// Retrieves the current version number of the School ID service.
        /// </summary>
        /// <returns>A string containing the current version number of the School ID service</returns>
        public string GetSchoolIDVersion()
        {
            PingOperation pingOperation = new PingOperation(schoolIDClient);
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
                RetrieveChainsOperation retrieveChainsOperation = new RetrieveChainsOperation(schoolIDClient);
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
                RetrieveSectorsOperation retrieveSectorsOperation = new RetrieveSectorsOperation(schoolIDClient);
                sectors = retrieveSectorsOperation.GetSectors();
            }

            return sectors;
        }

        /// <summary>
        /// Invokes the School ID service to generate a School ID based on the hashed PGN, Chain ID and Sector ID
        /// </summary>
        /// <param name="hpgn">The scrypt hashed PGN</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If no validation or operational errors, a School ID</returns>
        public string GenerateSchoolID(string hpgn, string chainGuid, string sectorGuid)
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            return retrieveEckIdOperation.GetEckId(hpgn, chainGuid, sectorGuid);
        }

        /// <summary>
        /// Invokes the School ID service to substitute the new HPGN with the old HPGN, in order to be able to keep using
        /// the old SchoolID even with a new BSN/PGN
        /// </summary>
        /// <param name="hpgnNew">The scrypt hashed new PGN, which will refer to the old HPGN instead</param>
        /// <param name="hpgnOld">The scrypt hashed old PGN, which the new HPGN will refer to</param>
        /// <param name="chainGuid">The specific, valid chain id for which the substitution applies</param>
        /// <param name="sectorGuid">The specific, valid sector id for which the substitution applies</param>
        /// <param name="effectiveDate">The date the substitution will become active (optional, default value: Now())</param>
        /// <returns>If no validation or operational errors, a School ID based on the active substitution(s)</returns>
        public string ReplaceEckID(string hpgnNew, string hpgnOld, string chainGuid, string sectorGuid, DateTime? effectiveDate)
        {
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);
            return replaceEckIdOperation.ReplaceEckId(hpgnNew, hpgnOld, chainGuid, sectorGuid, effectiveDate);
        }

        /// <summary>
        /// Translates the Dictionary to a ListedHpgn array and submits it to the Nummervoorziening service. The Batch Identifier is
        /// returned as a String if the submitting succeeded.
        /// </summary>
        /// <param name="listedHpgn">A list of indexed Hpgn for which a School ID is requested</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>If successful, a String containing the Batch Identifier</returns>
        public string SubmitHpgnBatch(Dictionary<int, string> hpgnDictionary, string chainGuid, string sectorGuid)
        {
            SubmitEckIdBatchOperation submitEckIdBatchOperation = new SubmitEckIdBatchOperation(schoolIDClient);
            return submitEckIdBatchOperation.SubmitHpgnBatch(hpgnDictionary, chainGuid, sectorGuid);
        }

        /// <summary>
        /// Fetches a Batch based with the given identifier. Incorporates cooldown periods as well as possible Faults.
        /// </summary>
        /// <param name="batchIdentifier">The identifier of the batch to retrieve</param>
        /// <returns>A populated SchoolIDBatch object</returns>
        public SchoolIDBatch RetrieveEckIdBatch(string batchIdentifier)
        {
            RetrieveEckIdBatchOperation retrieveEckIdBatchOperation = new RetrieveEckIdBatchOperation(schoolIDClient);
            return retrieveEckIdBatchOperation.RetrieveBatch(batchIdentifier);
        }
    }
}