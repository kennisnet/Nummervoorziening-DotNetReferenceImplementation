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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Demonstrates correct usage of the "Ping" operation
    /// </summary>
    [TestClass]
    public class PingOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests Nummervoorziening service availability.
        /// </summary>
        [TestMethod]
        public void GetAvailabilityTest()
        {
            Assert.AreEqual(true, EckIDServiceUtil.IsEckIdAvailable());
        }

        /// <summary>
        /// Tests correct version of Nummervoorziening service.
        /// </summary>
        [TestMethod]
        public void GetEckIdVersionTest()
        {
            string expectedValue = "1.0.4-SNAPSHOT";
            Assert.AreEqual(expectedValue, EckIDServiceUtil.GetEckIdVersion());
        }        
        
        /// <summary>
        /// Tests that time on server is not too different from local time.
        /// </summary>
        [TestMethod]
        public void GetEckIdDateTimeTest()
        {
            double allowedGapInMinutes = 180;

            DateTime? timeOnServer = EckIDServiceUtil.GetEckIdDateTime();
            DateTime currentDateTime = DateTime.Now;

            // Check if the DateTime we received is valid
            Assert.IsNotNull(timeOnServer);

            // Check if the gap between DateTime on the server and the local machine is acceptable
            TimeSpan timeGap = currentDateTime.Subtract((DateTime)timeOnServer);
            double gapInMinutes = Math.Abs(timeGap.TotalMinutes);
            Assert.IsTrue(gapInMinutes <= allowedGapInMinutes);
        }
    }
}
