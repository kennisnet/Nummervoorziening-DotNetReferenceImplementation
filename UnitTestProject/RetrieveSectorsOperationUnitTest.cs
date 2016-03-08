using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    [TestClass]
    public class RetrieveSectorsOperationUnitTest : AbstractUnitTest
    {           
        [TestMethod]
        public void GetSectorsTest()
        {
            RetrieveSectorsOperation retrieveSectorsOperation = new RetrieveSectorsOperation(schoolIDClient);
            Sector[] sectors = retrieveSectorsOperation.GetSectors();

            Assert.IsNotNull(sectors);
            Assert.IsTrue(sectors.Length > 0);
        }
    }
}
