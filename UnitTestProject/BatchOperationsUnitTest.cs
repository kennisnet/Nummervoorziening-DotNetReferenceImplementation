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

namespace UnitTestProject
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using EckID;

    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Batch" operation
    /// </summary>
    [TestClass]
    public class BatchOperationsUnitTest : AbstractUnitTest
    {
        private string INVALID_BATCH_IDENTIFIER = "invalid_batch_identifier";
        
        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves generated School IDs.
        /// </summary>
        [TestMethod]
        public void SimpleSubmittingAndRetrievingEckIdBatchTest()
        {     
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, ValidStudentStampseudonym);
            listedStampseudonym.Add(2, ValidTeacherStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, ValidChainGuid, ValidSectorGuid);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);
            
            // Test we received two EckIDs in the Success List and compare their contents with the expected values
            Assert.AreEqual(2, eckIdBatch.GetSuccessList().Count);
            Assert.AreEqual(ValidStudentEckID, eckIdBatch.GetSuccessList()[1]);
            Assert.AreEqual(ValidTeacherEckID, eckIdBatch.GetSuccessList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves generated stampseudonyms.
        /// </summary>
        [TestMethod]
        public void SimpleSubmittingAndRetrievingStampseudonymBatchTest()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, ValidStudentHpgn);
            listedHPgn.Add(2, ValidTeacherHpgn);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two stampseudonyms in the Success List and compare their contents with the expected values
            Assert.AreEqual(2, eckIdBatch.GetSuccessList().Count);
            Assert.AreEqual(ValidStudentStampseudonym, eckIdBatch.GetSuccessList()[1]);
            Assert.AreEqual(ValidTeacherStampseudonym, eckIdBatch.GetSuccessList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves failed items.
        /// </summary>
        [TestMethod]
        public void RetrievingBatchWithFailedItemsTest()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, InvalidStampseudonym);
            listedStampseudonym.Add(2, InvalidStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, ValidChainGuid, ValidSectorGuid);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two EckIDs in the Failure List and make sure the error message is not null
            Assert.AreEqual(2, eckIdBatch.GetFailedList().Count);
            Assert.IsNotNull(eckIdBatch.GetFailedList()[1]);
            Assert.IsNotNull(eckIdBatch.GetFailedList()[2]);
            Assert.IsTrue(eckIdBatch.GetSuccessList().Count == 0);
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
            listedStampseudonym.Add(1, ValidStudentStampseudonym);
            listedStampseudonym.Add(2, InvalidStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, ValidChainGuid, ValidSectorGuid);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two EckIDs, one in the Success List and one in the Failure List, 
            // and compare their contents with the expected values. Note that for retrieval from the appropriate Dictionaries
            // the original index is used as key.
            Assert.AreEqual(1, eckIdBatch.GetSuccessList().Count);
            Assert.AreEqual(1, eckIdBatch.GetFailedList().Count);
            Assert.AreEqual(ValidStudentEckID, eckIdBatch.GetSuccessList()[1]);
            Assert.IsNotNull(eckIdBatch.GetFailedList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves combination of
        /// processed and failed stampseudonyms.
        /// </summary>
        [TestMethod]
        public void RetrievingStampseudonymBatchWithFailedAndProcessedValues()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, ValidStudentHpgn);
            listedHPgn.Add(2, InvalidHpgn);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);

            // Test we received two stampseudonyms, one in the Success List and one in the Failure List, 
            // and compare their contents with the expected values. Note that for retrieval from the appropriate Dictionaries
            // the original index is used as key.
            Assert.AreEqual(1, eckIdBatch.GetSuccessList().Count);
            Assert.AreEqual(1, eckIdBatch.GetFailedList().Count);
            Assert.AreEqual(ValidStudentStampseudonym, eckIdBatch.GetSuccessList()[1]);
            Assert.IsNotNull(eckIdBatch.GetFailedList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving School ID batch content two times. A second retrieval of a 
        /// batch is not allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveEckIdBatchTwoTimesTest()
        {   
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedStampseudonym = new Dictionary<int, string>();
            listedStampseudonym.Add(1, ValidStudentStampseudonym);
            listedStampseudonym.Add(2, ValidTeacherStampseudonym);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitEckIdBatch(listedStampseudonym, ValidChainGuid, ValidSectorGuid);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);
            Assert.AreEqual(2, eckIdBatch.GetSuccessList().Count);

            // Retrieve the batch a second time
            EckIDServiceUtil.RetrieveBatch(batchIdentifier);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving stampseudonym batch content two times. A second retrieval of a 
        /// batch is not allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveStampseudonymBatchTwoTimesTest()
        {
            // Build a valid Stampseudonym batch to submit
            Dictionary<int, string> listedHPgn = new Dictionary<int, string>();
            listedHPgn.Add(1, ValidStudentHpgn);
            listedHPgn.Add(2, ValidTeacherHpgn);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = EckIDServiceUtil.SubmitStampseudonymBatch(listedHPgn);

            // Retrieve the batch
            EckIDBatch eckIdBatch = EckIDServiceUtil.RetrieveBatch(batchIdentifier);
            Assert.AreEqual(2, eckIdBatch.GetSuccessList().Count);

            // Retrieve the batch a second time
            EckIDServiceUtil.RetrieveBatch(batchIdentifier);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving batches with invalid identifiers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveBatchWithInvalidIdentifier()
        {            
            EckIDServiceUtil.RetrieveBatch(INVALID_BATCH_IDENTIFIER);
        }
    }
}