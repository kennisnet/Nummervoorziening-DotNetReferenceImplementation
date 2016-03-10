using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public abstract class AbstractUnitTest
    {
        protected SchoolIDClient schoolIDClient;

        /// <summary>
        /// Setups Service Util for working with Nummervoorziening service and disables SSL check (for now).
        /// </summary>
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
