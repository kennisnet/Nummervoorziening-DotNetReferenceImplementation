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
        private string INVALID_BATCH_IDENTIFIER = "invalid_batch_identifier";

        string validChainGuid = "http://purl.edustandaard.nl/begrippenkader/e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "http://purl.edustandaard.nl/begrippenkader/512e4729-03a4-43a2-95ba-758071d1b725";

        [TestMethod]
        public void SubmitValidHpgnBatchTest()
        {
            SubmitEckIdBatchOperation submitEckIdBatchOperation = new SubmitEckIdBatchOperation(schoolIDClient);
            RetrieveEckIdBatchOperation retrieveEckIdBatchOperation = new RetrieveEckIdBatchOperation(schoolIDClient);

            ScryptUtil scryptUtil = new ScryptUtil();
            SchoolIDBatch schoolIdBatch = new SchoolIDBatch();
            
            // Build a valid Hpgn batch to submit
            Dictionary<int, string> listedHpgn = new Dictionary<int, string>();
            listedHpgn.Add(1, scryptUtil.GenerateHexHash(validStudentPgn));
            listedHpgn.Add(2, scryptUtil.GenerateHexHash(validTeacherPgn));
            
            // Submit the batch, and fetch the identifier
            string batchIdentifier = submitEckIdBatchOperation.SubmitHpgnBatch(listedHpgn, validChainGuid, validSectorGuid);

            // Retrieve the batch
            schoolIdBatch = retrieveEckIdBatchOperation.RetrieveBatch(batchIdentifier);
            
            Assert.IsNotNull(schoolIdBatch);
            Assert.AreEqual(2, schoolIdBatch.getSuccessList().Count);
        }
    }
}
