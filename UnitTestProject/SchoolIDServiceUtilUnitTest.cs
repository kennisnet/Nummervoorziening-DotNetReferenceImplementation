using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID;

namespace UnitTestProject
{
    [TestClass]
    public class SchoolIDServiceUtilUnitTest : AbstractUnitTest
    {
        [TestMethod]
        public void CheckInstanceNotNullTest()
        {
            Assert.IsNotNull(schoolIDServiceUtil);
        }

        [TestMethod]
        public void CheckInstanceTypeTest()
        {
            Assert.IsInstanceOfType(schoolIDServiceUtil, typeof(SchoolIDServiceUtil));
        }

        [TestMethod]
        public void RecallSingletonTest()
        {
            SchoolIDServiceUtil schoolIDServiceUtil2 = SchoolIDServiceUtil.Instance;
            Assert.AreEqual(schoolIDServiceUtil, schoolIDServiceUtil2);
        }        
    }
}
