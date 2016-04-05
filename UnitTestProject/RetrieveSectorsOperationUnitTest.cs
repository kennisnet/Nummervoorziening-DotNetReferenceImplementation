using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Sectors" operation.
    /// </summary>
    [TestClass]
    public class RetrieveSectorsOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that the Nummervoorziening service returns non empty list of active sectors.
        /// </summary>
        [TestMethod]
        public void GetSectorsTest()
        {
            Sector[] sectors = schoolIDServiceUtil.GetSectors();

            Assert.IsNotNull(sectors);
            Assert.IsTrue(sectors.Length > 0);
        }
    }
}
