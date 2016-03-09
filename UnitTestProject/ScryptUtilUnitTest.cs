using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SCrypter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ScryptUtilUnitTest : AbstractUnitTest
    {
        string inputValue = "063138219";

        [TestMethod]
        public void GenerateBase64HashTest()
        {
            string expectedValue = "lSN80glj5jADRiAyRVCAmj35i74HdKNsNWv128imXns=";
            ScryptUtil scryptUtil = new ScryptUtil();

            Assert.AreEqual(expectedValue, scryptUtil.GenerateBase64Hash(inputValue));
        }

        [TestMethod]
        public void GenerateHexHashTest()
        {
            string expectedValue = "95237cd20963e630034620324550809a3df98bbe0774a36c356bf5dbc8a65e7b";
            ScryptUtil scryptUtil = new ScryptUtil();

            Assert.AreEqual(expectedValue, scryptUtil.GenerateHexHash(inputValue));
        }
    }
}
