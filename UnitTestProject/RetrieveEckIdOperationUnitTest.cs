using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;
using System.ServiceModel;

namespace UnitTestProject
{   
    /// <summary>
    /// Demonstrates the correct usage of the "Retrieve EckId" operation
    /// </summary>
    [TestClass]
    public class RetrieveEckIdOperationUnitTest : AbstractUnitTest
    {
        string validHpgn = "95237cd20963e630034620324550809a3df98bbe0774a36c356bf5dbc8a65e7b";
        string validPgn = "063138219";
        string validChainGuid;
        string validSectorGuid;

        string invalidHpgn = "";
        string invalidChainGuid = "invalidchainguid";
        string invalidSectorGuid = "invalidsectorguid";

        /// <summary>
        /// Initializes the test by retrieving valid Chains and Sectors from the Nummervoorziening service.
        /// </summary>
        [TestInitialize]
        public void GetEckIdInitializer()
        {
            // Retrieve a valid Chain from the Nummervoorziening service
            RetrieveChainsOperation retrieveChainsOperation = new RetrieveChainsOperation(schoolIDClient);
            validChainGuid = retrieveChainsOperation.GetChains()[0].id;

            // Retrieve a valid Sector from the Nummervoorziening service
            RetrieveSectorsOperation retrieveSectorsOperation = new RetrieveSectorsOperation(schoolIDClient);
            validSectorGuid = retrieveSectorsOperation.GetSectors()[0].id;
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidHpgnTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(invalidHpgn, validChainGuid, validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Chain Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidChainTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(validHpgn, invalidChainGuid, validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(validHpgn, validChainGuid, invalidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid parameters.
        /// </summary>
        [TestMethod]
        public void GetEckIdTest()
        {
            string expectedSchoolId = "https://school.id/pilot/998fc3e7c9add25be4369224e18d0876e7598480b184c6a35d8f49a49a3649040016f0aab6e292dd7da23292bd2f499e6018dfdab997d9408d80113d6dc72979";
            
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            string retrievedEckId = retrieveEckIdOperation.GetEckId(validHpgn, validChainGuid, validSectorGuid);
            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }
    }
}