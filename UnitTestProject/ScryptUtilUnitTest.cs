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
using NVA_DotNetReferenceImplementation.SCrypter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    /// <summary>
    /// Demonstrates correct usage of the ScryptUtil class.
    /// </summary>
    [TestClass]
    public class ScryptUtilUnitTest : AbstractUnitTest
    {
        string validStudentPgn = "063138219";
        string validTeacherPgn = "20DP teacher@school.com";

        /// <summary>
        /// Tests that generated scrypt hash in hexadecimal notation is correct.
        /// </summary>
        [TestMethod]
        public void GenerateStudentHexHashTest()
        {
            string expectedValue = "95237cd20963e630034620324550809a3df98bbe0774a36c356bf5dbc8a65e7b";
            ScryptUtil scryptUtil = new ScryptUtil();

            Assert.AreEqual(expectedValue, scryptUtil.GenerateHexHash(validStudentPgn));
        }

        /// <summary>
        /// Tests that generated scrypt hash in hexadecimal notation is correct.
        /// </summary>
        [TestMethod]
        public void GenerateTeacherHexHashTest()
        {
            string expectedValue = "4cadf651ec0197909e6432cb8347369adba39f44276a5b3cd59d17066f10ab3e";
            ScryptUtil scryptUtil = new ScryptUtil();

            Assert.AreEqual(expectedValue, scryptUtil.GenerateHexHash(validTeacherPgn));
        }

        /// <summary>
        /// Tests if input is lower-cased internally.
        /// </summary>
        [TestMethod]
        public void IsInputLowerCasedTest()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            Assert.AreEqual(scryptUtil.GenerateHexHash("INPUT"), scryptUtil.GenerateHexHash("input"));
        }
    }
}
