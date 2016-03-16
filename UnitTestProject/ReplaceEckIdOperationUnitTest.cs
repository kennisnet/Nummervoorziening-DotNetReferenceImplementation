using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;
using NVA_DotNetReferenceImplementation.SCrypter;
using System.ServiceModel;
using System;

namespace UnitTestProject
{   
    /// <summary>
    /// Demonstrates the correct usage of the "Retrieve EckId" operation
    /// </summary>
    [TestClass]
    public class ReplaceEckIdOperationUnitTest : AbstractUnitTest
    {
        private static string validHpgnNewPrefix = "csharp01";
        private static string validHpgnIntermediatePrefix = "csharp02";
        private static string validHpgnOldPrefix = "csharp03";
        private static string validHpgnNew;
        private static string validHpgnOld;

        string validChainGuid = "e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "512e4729-03a4-43a2-95ba-758071d1b725";

        string invalidHpgn = "";
        string invalidChainGuid = "invalidchainguid";
        string invalidSectorGuid = "invalidsectorguid";

        [ClassInitialize]
        public static void SetupValidHpgn(TestContext context)
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            validHpgnNew = scryptUtil.GenerateHexHash(validHpgnNewPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));
            validHpgnOld = scryptUtil.GenerateHexHash(validHpgnOldPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid new HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceEckIdInvalidNewHpgnTest()
        {
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);
            replaceEckIdOperation.ReplaceEckId(invalidHpgn, validHpgnOld, validChainGuid, validSectorGuid, null);            
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid old HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceEckIdInvalidOldHpgnTest()
        {
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);
            replaceEckIdOperation.ReplaceEckId(validHpgnNew, invalidHpgn, validChainGuid, validSectorGuid, null);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Chain Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceEckIdInvalidChainTest()
        {
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);
            replaceEckIdOperation.ReplaceEckId(validHpgnNew, validHpgnOld, invalidChainGuid, validSectorGuid, null);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceEckIdInvalidSectorTest()
        {
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);
            replaceEckIdOperation.ReplaceEckId(validHpgnNew, validHpgnOld, validChainGuid, invalidSectorGuid, null);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the retrieve Eck ID functionality. In this case, the
        /// substitution should be active immediately.
        /// </summary>
        [TestMethod]
        public void ReplaceEckIdNowTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);

            // Use the initial dataset to retrieve the Eck ID
            string initialEckId = retrieveEckIdOperation.GetEckId(validHpgnOld, validChainGuid, validSectorGuid);

            // Submit the substitution
            string processedEckId = replaceEckIdOperation.ReplaceEckId(validHpgnNew, validHpgnOld, validChainGuid, validSectorGuid, null);

            // Retrieve the Eck ID based on the new Hpgn, and check the result
            string finalEckId = retrieveEckIdOperation.GetEckId(validHpgnNew, validChainGuid, validSectorGuid);

            // Assert that the Eck ID retrieved from the Replace Eck ID operation is correct
            Assert.AreEqual(initialEckId, processedEckId);

            // Assert that he Eck ID retrieved based on the new Hpgn equals the old Hpgn
            Assert.AreEqual(initialEckId, finalEckId);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the retrieve Eck ID functionality. In this case, the
        /// substitution should not be active immediately.
        /// </summary>
        [TestMethod]
        public void ReplaceEckIdFutureTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);

            // Use a new set of values
            ScryptUtil scryptUtil = new ScryptUtil();
            string validFutureHpgnNew = scryptUtil.GenerateHexHash(validHpgnNewPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));
            string validFutureHpgnOld = scryptUtil.GenerateHexHash(validHpgnOldPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Use the initial dataset to retrieve the Eck ID
            string initialEckId = retrieveEckIdOperation.GetEckId(validFutureHpgnOld, validChainGuid, validSectorGuid);

            // Use the future Hpgn to retrieve the Eck ID based on the new Hpgn
            string newEckId = retrieveEckIdOperation.GetEckId(validFutureHpgnNew, validChainGuid, validSectorGuid);

            // Set the effectuation date to a moment in the future
            DateTime effectuationDate = DateTime.Now.AddYears(1);

            // Submit the substitution
            string processedEckId = replaceEckIdOperation.ReplaceEckId(validFutureHpgnNew, validFutureHpgnOld, validChainGuid, validSectorGuid, effectuationDate);

            // Retrieve the Eck ID based on the new Hpgn, and check the result
            string finalEckId = retrieveEckIdOperation.GetEckId(validFutureHpgnNew, validChainGuid, validSectorGuid);

            // Assert that the Eck ID retrieved from the Replace Eck ID operation is correct
            Assert.AreEqual(newEckId, processedEckId);

            // Assert that he Eck ID retrieved based on the new Hpgn equals the old Hpgn
            Assert.AreEqual(newEckId, finalEckId);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the retrieve Eck ID functionality. In this case, a
        /// substitution is submitted to substitution hpgn intermediate to hpgn old, and a second substitution from hpgn new 
        /// to hpgn intermediate. The last substitution should give back hpgn old instead of hpgn intermediate, also when
        /// retrieving the Eck Id based on hpgn new.
        /// </summary>
        [TestMethod]
        public void ReplaceEckIdWithIntermediateTest()
        {
            RetrieveEckIdOperation retrieveEckIdOperation = new RetrieveEckIdOperation(schoolIDClient);
            ReplaceEckIdOperation replaceEckIdOperation = new ReplaceEckIdOperation(schoolIDClient);

            // Use a new set of values
            ScryptUtil scryptUtil = new ScryptUtil();
            string validHpgnNew = scryptUtil.GenerateHexHash(validHpgnNewPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));
            string validHpgnIntermediate = scryptUtil.GenerateHexHash(validHpgnIntermediatePrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));
            string validHpgnOld = scryptUtil.GenerateHexHash(validHpgnOldPrefix + DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Use the datasets to retrieve the Eck IDs before substituting
            string oldEckId = retrieveEckIdOperation.GetEckId(validHpgnOld, validChainGuid, validSectorGuid);
            string intermediateEckId = retrieveEckIdOperation.GetEckId(validHpgnIntermediate, validChainGuid, validSectorGuid);

            // Submit the substitutions
            string eckIdFirstSubstitution = replaceEckIdOperation.ReplaceEckId(validHpgnIntermediate, validHpgnOld, validChainGuid, validSectorGuid, null);
            string eckIdSecondSubstitution = replaceEckIdOperation.ReplaceEckId(validHpgnNew, validHpgnIntermediate, validChainGuid, validSectorGuid, null);

            // Retrieve the Eck ID based on the new Hpgn, and check the result
            string finalEckId = retrieveEckIdOperation.GetEckId(validHpgnNew, validChainGuid, validSectorGuid);

            // Assert that the Eck ID retrieved from the first Replace Eck ID operation is correct
            Assert.AreEqual(oldEckId, eckIdFirstSubstitution);

            // Assert that the Eck ID retrieved from the second Replace Eck ID operation is correct
            Assert.AreEqual(oldEckId, eckIdSecondSubstitution);

            // Assert that he Eck ID retrieved based on the new Hpgn equals the old Hpgn
            Assert.AreEqual(oldEckId, finalEckId);
        }
    }
}