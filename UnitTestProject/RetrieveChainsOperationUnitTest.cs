using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    [TestClass]
    public class RetrieveChainsOperationUnitTest : AbstractUnitTest
    {           
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
