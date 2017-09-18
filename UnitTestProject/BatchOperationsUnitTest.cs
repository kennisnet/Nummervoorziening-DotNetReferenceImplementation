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

using NVA_DotNetReferenceImplementation.SCrypter;

namespace UnitTestProject
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NVA_DotNetReferenceImplementation.SchoolID;

    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Batch" operation
    /// </summary>
    [TestClass]
    public class BatchOperationsUnitTest : AbstractUnitTest
    {
        string validStudentStampseudonym = "https://id.school/sppilot/d0f58d6544562db32383d9fbd7e7d1c6857f9eb8fdaf43db9ac4fac8f3c6897cc6149985fe4a7b91b9be09a11c65b6bfd4d900357b0c96336b5521aaee261cf7";
        string validTeacherStampseudonym = "https://id.school/sppilot/e16ce3e75ee460e371972bb5b9f0ffee4e6bbdb0d3e0f059f7bf09592a70bb0a5cacf228ca8f1b855f280202c53cf8637c4a911d63969580aaae11ac72a33da4";
        string validStudentHpgn = "9735dfd2235eaeb5f0300886bcc99c82ffc1d6420c4e0bde8de7218def2135fa";
        string validTeacherHpgn = "0b870ff044775ef0360655c40d5b284b7e3ae2b72207a6894794d787eb019e60";
        string validStudentEckId = "https://id.school/pilot/e046daed612e3d3903792c3d7e74b2a6b5993cb1b2f6fec6767e41301d526ffec6082a8c2b3e999734eb4cfabb98297111e850bc41fa1f77b6b15c6a7c7d03dc";
        string validTeacherEckId = "https://id.school/pilot/903a199fde822072dc7ebd64a771f1e17c3d8223d155e0279d8fd1fe7075b67479521a32c9c2ebbd50c1169b53e3e92cfdda46baf87a54bb9a8314dbd6678424";
        string validChainGuid = "http://purl.edustandaard.nl/begrippenkader/e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "http://purl.edustandaard.nl/begrippenkader/512e4729-03a4-43a2-95ba-758071d1b725";

        private string INVALID_BATCH_IDENTIFIER = "invalid_batch_identifier";
        private string INVALID_STAMPSEUDONYM = "";
        private string INVALID_HPGN = "";

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves generated School IDs.
        /// </summary>
        [TestMethod]
        public void SimpleSubmittingAndRetrievingEckIdBatchTest()
        {     
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, this.validStudentStampseudonym);
            listedStampseudonym.Add(2, this.validTeacherStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = this.schoolIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = this.schoolIDServiceUtil.RetrieveBatch(batchIdentifier);
            
            // Test we received two EckIds in the Success List and compare their contents with the expected values
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(this.validStudentEckId, schoolIdBatch.getSuccessList()[1]);
            Assert.AreEqual(this.validTeacherEckId, schoolIdBatch.getSuccessList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves generated stampseudonyms.
        /// </summary>
        [TestMethod]
        public void SimpleSubmittingAndRetrievingStampseudonymBatchTest()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, validStudentHpgn);
            listedHPgn.Add(2, validTeacherHpgn);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two stampseudonyms in the Success List and compare their contents with the expected values
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(validStudentStampseudonym, schoolIdBatch.getSuccessList()[1]);
            Assert.AreEqual(validTeacherStampseudonym, schoolIdBatch.getSuccessList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves failed items.
        /// </summary>
        [TestMethod]
        public void RetrievingBatchWithFailedItemsTest()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, this.INVALID_STAMPSEUDONYM);
            listedStampseudonym.Add(2, this.INVALID_STAMPSEUDONYM);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = this.schoolIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, validChainGuid, validSectorGuid);

            // Retrieve the batch
            SchoolIDBatch schoolIdBatch = this.schoolIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two EckIds in the Failure List and make sure the error message is not null
            Assert.AreEqual(2, schoolIdBatch.getFailedList().Count);
            Assert.IsNotNull(schoolIdBatch.getFailedList()[1]);
            Assert.IsNotNull(schoolIdBatch.getFailedList()[2]);
            Assert.IsTrue(schoolIdBatch.getSuccessList().Count == 0);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves combination of
        /// processed and failed School IDs.
        /// </summary>
        [TestMethod]
        public void RetrievingEckIdBatchWithFailedAndProcessedValues()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, this.validStudentStampseudonym);
            listedStampseudonym.Add(2, this.INVALID_STAMPSEUDONYM);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = this.schoolIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, validChainGuid, validSectorGuid);

            // Retrieve the batch
            SchoolIDBatch schoolIdBatch = this.schoolIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two EckIds, one in the Success List and one in the Failure List, 
            // and compare their contents with the expected values. Note that for retrieval from the appropriate Dictionaries
            // the original index is used as key.
            Assert.AreEqual(1, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(1, schoolIdBatch.getFailedList().Count);
            Assert.AreEqual(this.validStudentEckId, schoolIdBatch.getSuccessList()[1]);
            Assert.IsNotNull(schoolIdBatch.getFailedList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves combination of
        /// processed and failed stampseudonyms.
        /// </summary>
        [TestMethod]
        public void RetrievingStampseudonymBatchWithFailedAndProcessedValues()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, validStudentHpgn);
            listedHPgn.Add(2, INVALID_HPGN);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two stampseudonyms, one in the Success List and one in the Failure List, 
            // and compare their contents with the expected values. Note that for retrieval from the appropriate Dictionaries
            // the original index is used as key.
            Assert.AreEqual(1, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(1, schoolIdBatch.getFailedList().Count);
            Assert.AreEqual(validStudentStampseudonym, schoolIdBatch.getSuccessList()[1]);
            Assert.IsNotNull(schoolIdBatch.getFailedList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving School ID batch content two times. A second retrieval of a 
        /// batch is not allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveEckIdBatchTwoTimesTest()
        {         
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, this.validStudentStampseudonym);
            listedStampseudonym.Add(2, this.validTeacherStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = this.schoolIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveBatch(batchIdentifier);
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);

            // Retrieve the batch a second time
            schoolIDServiceUtil.RetrieveBatch(batchIdentifier);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving stampseudonym batch content two times. A second retrieval of a 
        /// batch is not allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveStampseudonymBatchTwoTimesTest()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, validStudentHpgn);
            listedHPgn.Add(2, validTeacherHpgn);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveBatch(batchIdentifier);
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);

            // Retrieve the batch a second time
            this.schoolIDServiceUtil.RetrieveBatch(batchIdentifier);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving batches with invalid identifiers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveBatchWithInvalidIdentifier()
        {            
            this.schoolIDServiceUtil.RetrieveBatch(this.INVALID_BATCH_IDENTIFIER);
        }
    }
}