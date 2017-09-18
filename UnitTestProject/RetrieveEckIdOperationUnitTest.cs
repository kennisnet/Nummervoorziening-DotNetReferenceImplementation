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
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Demonstrates the correct usage of the "Retrieve EckId" operation
    /// </summary>
    [TestClass]
    public class RetrieveEckIdOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Stampseudonym based on PGN "063138219"
        /// </summary>
        private string validStudentStampseudonym = "https://id.school/sppilot/d0f58d6544562db32383d9fbd7e7d1c6857f9eb8fdaf43db9ac4fac8f3c6897cc6149985fe4a7b91b9be09a11c65b6bfd4d900357b0c96336b5521aaee261cf7";

        /// <summary>
        /// Stampseudonym based on PGN "20DP teacher@school.com"
        /// </summary>
        private string validTeacherStampseudonym = "https://id.school/sppilot/e16ce3e75ee460e371972bb5b9f0ffee4e6bbdb0d3e0f059f7bf09592a70bb0a5cacf228ca8f1b855f280202c53cf8637c4a911d63969580aaae11ac72a33da4";

        /// <summary>
        /// Valid SchoolId for student
        /// </summary>
        private string validStudentSchoolId = "https://id.school/pilot/e046daed612e3d3903792c3d7e74b2a6b5993cb1b2f6fec6767e41301d526ffec6082a8c2b3e999734eb4cfabb98297111e850bc41fa1f77b6b15c6a7c7d03dc";

        /// <summary>
        /// Valid SchoolId for student
        /// </summary>
        private string validTeacherSchoolId = "https://id.school/pilot/903a199fde822072dc7ebd64a771f1e17c3d8223d155e0279d8fd1fe7075b67479521a32c9c2ebbd50c1169b53e3e92cfdda46baf87a54bb9a8314dbd6678424";

        /// <summary>
        /// A valid chain guid
        /// </summary>
        private string validChainGuid = "http://purl.edustandaard.nl/begrippenkader/e7ec7d3c-c235-4513-bfb6-e54e66854795";

        /// <summary>
        /// A valid sector guid
        /// </summary>
        private string validSectorGuid = "http://purl.edustandaard.nl/begrippenkader/512e4729-03a4-43a2-95ba-758071d1b725";

        /// <summary>
        /// An invalid hpgn (empty)
        /// </summary>
        private string invalidStampseudonym = string.Empty;

        /// <summary>
        /// An invalid chain guid
        /// </summary>
        private string invalidChainGuid = "invalidchainguid";

        /// <summary>
        /// An invalid sector guid
        /// </summary>
        private string invalidSectorGuid = "invalidsectorguid";

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidStampseudonymTest()
        {
            this.schoolIDServiceUtil.GenerateSchoolID(this.invalidStampseudonym, this.validChainGuid, this.validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Chain Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidChainTest()
        {
            this.schoolIDServiceUtil.GenerateSchoolID(this.validStudentStampseudonym, this.invalidChainGuid, this.validSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            this.schoolIDServiceUtil.GenerateSchoolID(this.validStudentStampseudonym, this.validChainGuid, this.invalidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentSchoolIdTest()
        {
            string retrievedSchoolId = this.schoolIDServiceUtil.GenerateSchoolID(this.validStudentStampseudonym, this.validChainGuid, this.validSectorGuid);
            Assert.AreEqual(this.validStudentSchoolId, retrievedSchoolId);
        }
        
        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherSchoolIdTest()
        {
            string retrievedSchoolId = this.schoolIDServiceUtil.GenerateSchoolID(this.validTeacherStampseudonym, this.validChainGuid, this.validSectorGuid);
            Assert.AreEqual(this.validTeacherSchoolId, retrievedSchoolId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid teacher parameters that based on output of GenerateStampseudonym service.
        /// </summary>
        [TestMethod]
        public void GetStudentSchoolIdAfterRetrieveStampseudonymTest()
        {
            string validStudentHpgn = "9735dfd2235eaeb5f0300886bcc99c82ffc1d6420c4e0bde8de7218def2135fa";
            string stampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validStudentHpgn);  
            
            string retrievedSchoolId = this.schoolIDServiceUtil.GenerateSchoolID(stampseudonym, this.validChainGuid, this.validSectorGuid);
            Assert.AreEqual(this.validStudentSchoolId, retrievedSchoolId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct SchoolID on valid student parameters that based on output of GenerateStampseudonym service.
        /// </summary>
        [TestMethod]
        public void GetTeacherSchoolIdAfterRetrieveStampseudonymTest()
        {
            string validTeacherHpgn = "0b870ff044775ef0360655c40d5b284b7e3ae2b72207a6894794d787eb019e60";
            string stampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validTeacherHpgn);

            string retrievedSchoolId = this.schoolIDServiceUtil.GenerateSchoolID(stampseudonym, this.validChainGuid, this.validSectorGuid);
            Assert.AreEqual(this.validTeacherSchoolId, retrievedSchoolId);
        }
    }
}