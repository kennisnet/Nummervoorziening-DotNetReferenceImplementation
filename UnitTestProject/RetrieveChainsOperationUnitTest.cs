using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Chains" operation.
    /// </summary>
    [TestClass]
    public class RetrieveChainsOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that the Nummervoorziening service returns non empty list of active chains.
        /// </summary>
        [TestMethod]
        public void GetChainsTest()
        {
            RetrieveChainsOperation retrieveChainsOperation = new RetrieveChainsOperation(schoolIDClient);
            Chain[] chains = retrieveChainsOperation.GetChains();

            Assert.IsNotNull(chains);
            Assert.IsTrue(chains.Length > 0);
        }
    }
}
