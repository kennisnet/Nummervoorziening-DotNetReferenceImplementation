using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    [TestClass]
    public class PingOperationUnitTest : AbstractUnitTest
    {   
        [TestMethod]
        public void GetSchoolIDVersionTest()
        {
            string expectedValue = "0.1.0-SNAPSHOT";

            PingOperation pingOperation = new PingOperation(schoolIDClient);
            Assert.AreEqual(expectedValue, pingOperation.GetSchoolIDVersion());
        }

        [TestMethod]
        public void GetAvailabilityTest()
        {
            bool expectedValue = true;

            PingOperation pingOperation = new PingOperation(schoolIDClient);
            Assert.AreEqual(expectedValue, pingOperation.IsAvailable());
        }

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
