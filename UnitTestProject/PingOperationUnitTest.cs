using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
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
            bool expectedValue = true;

            PingOperation pingOperation = new PingOperation(schoolIDClient);
            Assert.AreEqual(expectedValue, pingOperation.IsAvailable());
        }

        /// <summary>
        /// Tests correct version of Nummervoorziening service.
        /// </summary>
        [TestMethod]
        public void GetSchoolIDVersionTest()
        {
            string expectedValue = "0.1.0-SNAPSHOT";

            PingOperation pingOperation = new PingOperation(schoolIDClient);
            Assert.AreEqual(expectedValue, pingOperation.GetSchoolIDVersion());
        }        
        
        /// <summary>
        /// Tests that time on server is not too different from local time.
        /// </summary>
        [TestMethod]
        public void GetSchoolIDDateTimeTest()
        {
            double allowedGapInMinutes = 180;

            PingOperation pingOperation = new PingOperation(schoolIDClient);
            DateTime? timeOnServer = pingOperation.GetSchoolIDDateTime();
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
