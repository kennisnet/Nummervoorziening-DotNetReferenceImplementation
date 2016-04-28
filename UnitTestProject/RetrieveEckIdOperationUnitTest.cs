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

        string validChainGuid = "http://purl.edustandaard.nl/begrippenkader/e7ec7d3c-c235-4513-bfb6-e54e66854795";
        string validSectorGuid = "http://purl.edustandaard.nl/begrippenkader/512e4729-03a4-43a2-95ba-758071d1b725";
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
            schoolIDServiceUtil.GenerateSchoolID(invalidHpgn, validChainGuid, validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Chain Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidChainTest()
        {
            schoolIDServiceUtil.GenerateSchoolID(validStudentHpgn, invalidChainGuid, validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            schoolIDServiceUtil.GenerateSchoolID(validStudentHpgn, validChainGuid, invalidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentSchoolIdTest()
        {
            string expectedSchoolId = "https://id.school/pilot/a7d5e96cbfc61cddcf9a918150d5137c6659497ecb435d97abfc60b7297c750a47a3163af49418acc73148d34915833b1cef077ba687c621aa40654906073571";
            string retrievedEckId = schoolIDServiceUtil.GenerateSchoolID(validStudentHpgn, validChainGuid, validSectorGuid);

            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentSchoolIdUppercaseTest()
        {
            string expectedSchoolId = "https://id.school/pilot/a7d5e96cbfc61cddcf9a918150d5137c6659497ecb435d97abfc60b7297c750a47a3163af49418acc73148d34915833b1cef077ba687c621aa40654906073571";
            string retrievedEckId = schoolIDServiceUtil.GenerateSchoolID(validStudentHpgn, validChainGuid, validSectorGuid);

            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherSchoolIdTest()
        {
            string expectedSchoolId = "https://id.school/pilot/8dc3d9adad74ee2d588a6456be26da9faab1f0b1801bb15897f0e979ada55556aee041e329b27328259ba383af779080209c5c54f3db9b171bd43980aedc47c3";
            string retrievedEckId = schoolIDServiceUtil.GenerateSchoolID(validTeacherHpgn, validChainGuid, validSectorGuid);

            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }
    }
}