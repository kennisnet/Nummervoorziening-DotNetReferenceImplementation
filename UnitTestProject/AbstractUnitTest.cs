using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    [TestClass]
    public abstract class AbstractUnitTest
    {
        protected SchoolIDClient schoolIDClient;

        [TestInitialize()]
        public void initializeSchoolIDClientAndDisableSSL()
        {
            // Disable SSL checks for now
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            schoolIDClient = new SchoolIDClient();
        }
    }
}
