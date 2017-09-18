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
    using System;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NVA_DotNetReferenceImplementation.SCrypter;

    /// <summary>
    /// Demonstrates the correct usage of the "Retrieve EckId" operation
    /// </summary>
    [TestClass]
    public class ReplaceStampseudonymOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Valid hPGN prefix for hPGN new
        /// </summary>
        private static string validHpgnNewPrefix = "csharp01";

        /// <summary>
        /// Valid hPGN prefix for hPGN intermediate
        /// </summary>
        private static string validHpgnIntermediatePrefix = "csharp02";

        /// <summary>
        /// Valid hPGN prefix for hPGN old
        /// </summary>
        private static string validHpgnOldPrefix = "csharp03";

        /// <summary>
        /// Valid value for hPGN new
        /// </summary>
        private static string validHpgnNew;

        /// <summary>
        /// Valid value for hPGN intermediate
        /// </summary>
        private static string validHpgnIntermediate;

        /// <summary>
        /// Valid value for hPGN old 
        /// </summary>
        private static string validHpgnOld;

        /// <summary>
        /// Sequence counter
        /// </summary>
        private static int sequenceCounter;

        /// <summary>
        /// Invalid hPGN, in this case, empty String
        /// </summary>
        private readonly string invalidHpgn = string.Empty;

        /// <summary>
        /// Get a unique sequential number
        /// </summary>
        /// <returns>The next available number</returns>
        public static int GetSequentialNumber()
        {
            return sequenceCounter++;
        }

        /// <summary>
        /// Setup a valid hpgn. For this test, the PGN is extended with a sequential number and a date string. This is for 
        /// demonstration purpose only! On production environment, only PGN with no extensions should be used as the input 
        /// for the Scrypt operation.
        /// </summary>
        [TestInitialize]
        public void SetupValidHpgn()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            validHpgnNew = scryptUtil.GenerateHexHash(validHpgnNewPrefix + GetSequentialNumber() + 
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            validHpgnIntermediate = scryptUtil.GenerateHexHash(validHpgnIntermediatePrefix + GetSequentialNumber() +
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            validHpgnOld = scryptUtil.GenerateHexHash(validHpgnOldPrefix + GetSequentialNumber() + 
                DateTime.Now.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid new HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceStampseudonymInvalidNewHpgnTest()
        {
            this.schoolIDServiceUtil.ReplaceStampseudonym(this.invalidHpgn, validHpgnOld, null);            
        }

        /// <summary>
        /// Tests that Nummervoorziening service throws error on invalid old HPgn.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public void ReplaceStampseudonymInvalidOldHpgnTest()
        {
            this.schoolIDServiceUtil.ReplaceStampseudonym(validHpgnNew, this.invalidHpgn, null);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the
        /// generate stampseudonym functionality. In this case, the substitution
        /// should be active immediately
        /// </summary>
        [TestMethod]
        public void ReplaceStampseudonymNowTest()
        {
            // Use the initial dataset to retrieve the stampseudonym
            string initialStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnOld);

            // Submit the substitution
            string processedStampseudonym = this.schoolIDServiceUtil.ReplaceStampseudonym(validHpgnNew, validHpgnOld, null);

            // Retrieve the stampseudonym based on the new Hpgn, and check the result
            string finalStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnNew);

            // Assert that the stampseudonym retrieved from the Replace Stampseudonym operation is correct
            Assert.AreEqual(initialStampseudonym, processedStampseudonym);

            // Assert that the stampseudonym retrieved based on the new Hpgn equals the old Hpgn
            Assert.AreEqual(initialStampseudonym, finalStampseudonym);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the retrieve stampseudonym functionality. In this case,
        /// a substitution is submitted to substitution hpgn intermediate to hpgn old, and a second substitution from hpgn new
        /// to hpgn intermediate. The last substitution should give back hpgn old instead of hpgn intermediate, also when
        /// retrieving the Eck Id based on hpgn new.
        /// </summary>
        [TestMethod]
        public void ReplaceStampseudonymWithIntermediateTest()
        {
            // Use the datasets to retrieve the stampseudonym before substituting
            string oldStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnOld);

            // Submit the substitutions
            string stampseudonymFirstSubstitution = this.schoolIDServiceUtil.ReplaceStampseudonym(validHpgnIntermediate,
                validHpgnOld, null);
            string stampseudonymSecondSubstitution = this.schoolIDServiceUtil.ReplaceStampseudonym(validHpgnNew,
                validHpgnIntermediate, null);

            // Retrieve the stampseudonym based on the new Hpgn, and check the result
            string finalStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnNew);

            // Assert that the stampseudonym retrieved from the first Replace Stampseudonym operation is correct
            Assert.AreEqual(oldStampseudonym, stampseudonymFirstSubstitution);

            // Assert that the stampseudonym retrieved from the second Replace Stampseudonym operation is correct
            Assert.AreEqual(oldStampseudonym, stampseudonymSecondSubstitution);

            // Assert that he stampseudonym retrieved based on the new Hpgn equals the old Hpgn
            Assert.AreEqual(oldStampseudonym, finalStampseudonym);
        }

        /// <summary>
        /// Tests the Substitution functionality based on the output of the retrieve
        /// Stampseudonym functionality. In this case, the substitution should not be
        /// active immediately.
        /// </summary>
        [TestMethod]
        public void ReplaceStampseudonymFutureTest()
        {
            // Use the future Hpgn to retrieve the stampseudonym based on the new Hpgn
            string newStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnNew);

            // Set the effectuation date to a moment in the future
            DateTime effectuationDate = DateTime.Now.AddYears(1);

            // Submit the substitution
            string processedStampseudonym = this.schoolIDServiceUtil.ReplaceStampseudonym(validHpgnNew, validHpgnOld, effectuationDate);

            // Retrieve the stampseudonym based on the new Hpgn, and check the result
            string finalStampseudonym = this.schoolIDServiceUtil.GenerateStampseudonym(validHpgnNew);

            // Assert that the stampseudonym retrieved from the Replace Stampseudonym operation is correct
            Assert.AreEqual(newStampseudonym, processedStampseudonym);

            // Assert that the stampseudonym retrieved based on the new Hpgn equals the original value (thus not being substituted)
            Assert.AreEqual(newStampseudonym, finalStampseudonym);
        }
}
}