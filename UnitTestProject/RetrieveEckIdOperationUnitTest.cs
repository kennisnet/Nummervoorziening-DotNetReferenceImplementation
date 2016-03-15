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
        // HPgn based on PGN "063138219"
        string validStudentHpgn = "95237cd20963e630034620324550809a3df98bbe0774a36c356bf5dbc8a65e7b";

        // HPgn based on PGN "20DP teacher@school.com"
        string validTeacherHpgn = "4cadf651ec0197909e6432cb8347369adba39f44276a5b3cd59d17066f10ab3e";

        string validChainGuid = "e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "512e4729-03a4-43a2-95ba-758071d1b725";

        string invalidHpgn = "";
        string invalidChainGuid = "invalidchainguid";
        string invalidSectorGuid = "invalidsectorguid";

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
            retrieveEckIdOperation.GetEckId(validStudentHpgn, invalidChainGuid, validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            retrieveEckIdOperation.GetEckId(validStudentHpgn, validChainGuid, invalidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentSchoolIdTest()
        {
            string expectedSchoolId = "https://id.school/pilot/998fc3e7c9add25be4369224e18d0876e7598480b184c6a35d8f49a49a3649040016f0aab6e292dd7da23292bd2f499e6018dfdab997d9408d80113d6dc72979";
            
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            string retrievedEckId = retrieveEckIdOperation.GetEckId(validStudentHpgn, validChainGuid, validSectorGuid);
            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherSchoolIdTest()
        {
            string expectedSchoolId = "https://id.school/pilot/2650076c96066464e76063f92c6dd59c46bca515d9e7c0c8dd9ae1c1b733751a3ab20b50688b39dc633a04dbefc76ac2bbbd9e62abe3b68558dbbcb831148d62";

            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            string retrievedEckId = retrieveEckIdOperation.GetEckId(validTeacherHpgn, validChainGuid, validSectorGuid);
            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }
    }
}