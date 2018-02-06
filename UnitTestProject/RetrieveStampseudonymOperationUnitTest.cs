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
    public class RetrieveStampseudonymOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void GetStampseudoniemInvalidHpgnTest()
        {
            EckIDServiceUtil.GenerateStampseudonym(InvalidHpgn);
        }
        
        /// <summary>
        /// Tests that Nummervoorziening service returns correct Stampseudoniem on valid student parameters.
        /// </summary>
        [TestMethod]
        public void GetStudentStampseudoniemTest()
        {
            string retrievedEckID = EckIDServiceUtil.GenerateStampseudonym(ValidStudentHpgn);
            Assert.AreEqual(ValidStudentStampseudonym, retrievedEckID);
        }

        /// <summary>
        /// Tests that Nummervoorziening service returns correct Stampseudoniem on valid teacher parameters.
        /// </summary>
        [TestMethod]
        public void GetTeacherStampseudoniemTest()
        {
            string retrievedEckID = EckIDServiceUtil.GenerateStampseudonym(ValidTeacherHpgn);
            Assert.AreEqual(ValidTeacherStampseudonym, retrievedEckID);
        }
    }
}