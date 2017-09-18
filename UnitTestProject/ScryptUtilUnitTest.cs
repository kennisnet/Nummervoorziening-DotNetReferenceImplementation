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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NVA_DotNetReferenceImplementation.SCrypter;

    /// <summary>
    /// Demonstrates correct usage of the ScryptUtil class.
    /// </summary>
    [TestClass]
    public class ScryptUtilUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Standard student pgn to use for the tests
        /// </summary>
        private string validStudentPgn = "063138219";

        /// <summary>
        /// Standard teacher pgn to use
        /// </summary>
        private string validTeacherPgn = "20DP teacher@school.com";

        /// <summary>
        /// Tests that generated scrypt hash in hexadecimal notation is correct.
        /// </summary>
        [TestMethod]
        public void GenerateStudentHexHashTest()
        {
            string expectedValue = "9735dfd2235eaeb5f0300886bcc99c82ffc1d6420c4e0bde8de7218def2135fa";
            ScryptUtil scryptUtil = new ScryptUtil();
            Assert.AreEqual(expectedValue, scryptUtil.GenerateHexHash(this.validStudentPgn));
        }

        /// <summary>
        /// Tests that generated scrypt hash in hexadecimal notation is correct.
        /// </summary>
        [TestMethod]
        public void GenerateTeacherHexHashTest()
        {
            string expectedValue = "0b870ff044775ef0360655c40d5b284b7e3ae2b72207a6894794d787eb019e60";
            ScryptUtil scryptUtil = new ScryptUtil();
            Assert.AreEqual(expectedValue, scryptUtil.GenerateHexHash(this.validTeacherPgn));
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
