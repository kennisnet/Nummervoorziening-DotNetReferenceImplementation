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
    /// Demonstrates the correct usage of the "Retrieve EckID" operation
    /// </summary>
    [TestClass]
    public class RetrieveEckIdOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidStampseudonymTest()
        {
            EckIDServiceUtil.GenerateEckId(InvalidStampseudonym, ValidChainGuid, ValidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Chain Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidChainTest()
        {
            EckIDServiceUtil.GenerateEckId(ValidStudentStampseudonym, InvalidChainGuid, ValidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid Sector Guid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetEckIdInvalidSectorTest()
        {
            EckIDServiceUtil.GenerateEckId(ValidStudentStampseudonym, ValidChainGuid, InvalidSectorGuid);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct EckID on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentEckIdTest()
        {
            string retrievedEckId = EckIDServiceUtil.GenerateEckId(ValidStudentStampseudonym, ValidChainGuid, ValidSectorGuid);
            Assert.AreEqual(ValidStudentEckID, retrievedEckId);
        }
        
        /// <summary>
        /// Tests that Nummervoorziening service returns correct EckID on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherEckIdTest()
        {
            string retrievedEckId = EckIDServiceUtil.GenerateEckId(ValidTeacherStampseudonym, ValidChainGuid, ValidSectorGuid);
            Assert.AreEqual(ValidTeacherEckID, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct EckID on valid teacher parameters that based on output of GenerateStampseudonym service.
        /// </summary>
        [TestMethod]
        public void GetStudentEckIdAfterRetrieveStampseudonymTest()
        {
            string stampseudonym = EckIDServiceUtil.GenerateStampseudonym(ValidStudentHpgn);  
            string retrievedEckId = EckIDServiceUtil.GenerateEckId(stampseudonym, ValidChainGuid, ValidSectorGuid);
            Assert.AreEqual(ValidStudentEckID, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct EckID on valid student parameters that based on output of GenerateStampseudonym service.
        /// </summary>
        [TestMethod]
        public void GetTeacherEckIdAfterRetrieveStampseudonymTest()
        {
            string stampseudonym = EckIDServiceUtil.GenerateStampseudonym(ValidTeacherHpgn);
            string retrievedEckId = EckIDServiceUtil.GenerateEckId(stampseudonym, ValidChainGuid, ValidSectorGuid);
            Assert.AreEqual(ValidTeacherEckID, retrievedEckId);
        }
    }
}