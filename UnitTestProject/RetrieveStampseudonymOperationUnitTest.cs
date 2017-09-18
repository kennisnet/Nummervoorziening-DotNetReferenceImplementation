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
    public class RetrieveStampseudonymOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// An invalid hpgn (empty)
        /// </summary>
        private readonly string invalidHpgn = string.Empty;

        /// <summary>
        /// HPgn based on PGN "063138219"
        /// </summary>
        private string validStudentHpgn = "9735dfd2235eaeb5f0300886bcc99c82ffc1d6420c4e0bde8de7218def2135fa";

        /// <summary>
        /// HPgn based on PGN "20DP teacher@school.com"
        /// </summary>
        private string validTeacherHpgn = "0b870ff044775ef0360655c40d5b284b7e3ae2b72207a6894794d787eb019e60";
        
        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetStampseudoniemInvalidHpgnTest()
        {
            this.schoolIDServiceUtil.GenerateStampseudonym(this.invalidHpgn);
        }
        
        /// <summary>
        /// Tests that Nummervoorziening service returns correct Stampseudoniem on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentStampseudoniemTest()
        {
            string expectedSchoolId = "https://id.school/sppilot/d0f58d6544562db32383d9fbd7e7d1c6857f9eb8fdaf43db9ac4fac8f3c6897cc6149985fe4a7b91b9be09a11c65b6bfd4d900357b0c96336b5521aaee261cf7";
            string retrievedEckId = this.schoolIDServiceUtil.GenerateStampseudonym(this.validStudentHpgn);

            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct Stampseudoniem on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherStampseudoniemTest()
        {
            string expectedSchoolId = "https://id.school/sppilot/e16ce3e75ee460e371972bb5b9f0ffee4e6bbdb0d3e0f059f7bf09592a70bb0a5cacf228ca8f1b855f280202c53cf8637c4a911d63969580aaae11ac72a33da4";
            string retrievedEckId = this.schoolIDServiceUtil.GenerateStampseudonym(this.validTeacherHpgn);

            Assert.AreEqual(expectedSchoolId, retrievedEckId);
        }
    }
}