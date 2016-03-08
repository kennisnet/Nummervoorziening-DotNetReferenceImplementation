using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;
using System.ServiceModel;
using NVA_DotNetReferenceImplementation.SCrypter;

namespace UnitTestProject
{   
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

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidHpgnTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(invalidHpgn, validChainGuid, validSectorGuid);
        }
                
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidChainTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(validHpgn, invalidChainGuid, validSectorGuid);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(validHpgn, validChainGuid, invalidSectorGuid);
        }
        
        [TestMethod]
        public void GetEckIdTest()
        {
            string expectedEckId = "https://school.id/pilot/998fc3e7c9add25be4369224e18d0876e7598480b184c6a35d8f49a49a3649040016f0aab6e292dd7da23292bd2f499e6018dfdab997d9408d80113d6dc72979";
            
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            string retrievedEckId = retrieveEckIdOperation.GetEckId(validHpgn, validChainGuid, validSectorGuid);
            Assert.AreEqual(expectedEckId, retrievedEckId);
        }
    }
}