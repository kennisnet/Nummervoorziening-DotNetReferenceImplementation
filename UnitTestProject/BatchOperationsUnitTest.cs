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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;
using System.Collections.Generic;
using NVA_DotNetReferenceImplementation.SCrypter;
using NVA_DotNetReferenceImplementation.SchoolID;
using System.ServiceModel;
using System.Threading;

namespace UnitTestProject
{
    [TestClass]
    public class BatchOperationsUnitTest : AbstractUnitTest
    {
        string validStudentPgn = "063138219";
        string validTeacherPgn = "20DP teacher@school.com";
        string validStudentEckId = "https://id.school/pilot/a7d5e96cbfc61cddcf9a918150d5137c6659497ecb435d97abfc60b7297c750a47a3163af49418acc73148d34915833b1cef077ba687c621aa40654906073571";
        string validTeacherEckId = "https://id.school/pilot/8dc3d9adad74ee2d588a6456be26da9faab1f0b1801bb15897f0e979ada55556aee041e329b27328259ba383af779080209c5c54f3db9b171bd43980aedc47c3";
        string validChainGuid = "http://purl.edustandaard.nl/begrippenkader/e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "http://purl.edustandaard.nl/begrippenkader/512e4729-03a4-43a2-95ba-758071d1b725";


        private string INVALID_BATCH_IDENTIFIER = "invalid_batch_identifier";
        private string INVALID_HPGN = "";

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves generated School IDs.
        /// </summary>
        [TestMethod]
        public void SimpleSubmittingAndRetrievingBatchTest()
        {     
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();
            
            // Build a valid Hpgn batch to submit
            Dictionary<int, string> listedHpgn = new Dictionary<int, string>();
            listedHpgn.Add(1, scryptUtil.GenerateHexHash(validStudentPgn));
            listedHpgn.Add(2, scryptUtil.GenerateHexHash(validTeacherPgn));
            
            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitHpgnBatch(listedHpgn, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveEckIdBatch(batchIdentifier);
            
            // Test we received two EckIds in the Success List and compare their contents with the expected values
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(validStudentEckId, schoolIdBatch.getSuccessList()[1]);
            Assert.AreEqual(validTeacherEckId, schoolIdBatch.getSuccessList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service correctly retrieves failed items.
        /// </summary>
        [TestMethod]
        public void RetrievingBatchWithFailedItemsTest()
        {     
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Hpgn batch to submit
            Dictionary<int, string> listedHpgn = new Dictionary<int, string>();
            listedHpgn.Add(1, INVALID_HPGN);
            listedHpgn.Add(2, INVALID_HPGN);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitHpgnBatch(listedHpgn, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveEckIdBatch(batchIdentifier);

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
        public void RetrievingBatchWithFailedAndProcessedValues()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Hpgn batch to submit
            Dictionary<int, string> listedHpgn = new Dictionary<int, string>();
            listedHpgn.Add(1, scryptUtil.GenerateHexHash(validStudentPgn));
            listedHpgn.Add(2, INVALID_HPGN);

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitHpgnBatch(listedHpgn, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveEckIdBatch(batchIdentifier);

            // Test we received two EckIds, one in the Success List and one in the Failure List, 
            // and compare their contents with the expected values. Note that for retrieval from the appropriate Dictionaries
            // the original index is used as key.
            Assert.AreEqual(1, schoolIdBatch.getSuccessList().Count);
            Assert.AreEqual(1, schoolIdBatch.getFailedList().Count);
            Assert.AreEqual(validStudentEckId, schoolIdBatch.getSuccessList()[1]);
            Assert.IsNotNull(schoolIdBatch.getFailedList()[2]);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving batch content two times. A second retrieval of a 
        /// batch is not allowed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveBatchTwoTimesTest()
        {         
            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();

            // Build a valid Hpgn batch to submit
            Dictionary<int, string> listedHpgn = new Dictionary<int, string>();
            listedHpgn.Add(1, scryptUtil.GenerateHexHash(validStudentPgn));
            listedHpgn.Add(2, scryptUtil.GenerateHexHash(validTeacherPgn));

            // Submit the batch, and fetch the identifier
            string batchIdentifier = schoolIDServiceUtil.SubmitHpgnBatch(listedHpgn, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = schoolIDServiceUtil.RetrieveEckIdBatch(batchIdentifier);
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);

            // Retrieve the batch a second time
            schoolIDServiceUtil.RetrieveEckIdBatch(batchIdentifier);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on retrieving batches with invalid identifiers.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void RetrieveBatchWithInvalidIdentifier()
        {            
            schoolIDServiceUtil.RetrieveEckIdBatch(INVALID_BATCH_IDENTIFIER);
        }
    }
}
