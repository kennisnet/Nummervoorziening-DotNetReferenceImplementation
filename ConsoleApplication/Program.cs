using NVA_DotNetReferenceImplementation.SchoolID;
using NVA_DotNetReferenceImplementation.SCrypter;
using System;

namespace ConsoleNVAClient
{
    class Program
    {
        private static SchoolIDServiceUtil schoolIDServiceUtil;
        private static Chain[] chains;
        private static Sector[] sectors;
        
        static void Main(string[] args)
        {
            // Disable SSL checks for now
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            // Setup the Service Utility for School ID 
            schoolIDServiceUtil = SchoolIDServiceUtil.Instance;

            // Status information
            if (schoolIDServiceUtil.IsSchoolIDAvailable())
            {
                WritePingStatusOutput();               
                WriteAvailableChains();
                WriteAvailableSectors();                
                
                // Tests                
                ExecuteClientTests();                
            } else
            {
                Console.WriteLine("School ID service is offline.");
            }

            Console.ReadLine();
        }

        private static void WritePingStatusOutput()
        {
            Console.WriteLine("School ID service is online.");
            Console.WriteLine("Current version: " + schoolIDServiceUtil.GetSchoolIDVersion());
            Console.WriteLine("Current DateTime of School ID server: " + schoolIDServiceUtil.GetSchoolIDDateTime());
            Console.WriteLine();
        }

        private static void WriteAvailableChains()
        {
            chains = schoolIDServiceUtil.GetChains();

            // List available Chains
            foreach (Chain chain in chains)
            {
                Console.WriteLine(chain.name + " (" + chain.id + "): " + chain.description);
            }
            Console.WriteLine();
        }

        private static void WriteAvailableSectors()
        {
            sectors = schoolIDServiceUtil.GetSectors();

            // List available Sectors
            foreach (Sector sector in sectors)
            {
                Console.WriteLine(sector.name + " (" + sector.id + "): " + sector.description);
            }

            Console.WriteLine();
        }

        private static void WriteHashExample()
        {
            ScryptUtil scryptUtil = new ScryptUtil();
            byte[] hash = scryptUtil.GenerateHash("secret");
            Console.WriteLine("Base64: " + Convert.ToBase64String(hash));
            Console.WriteLine("Base16: " + BitConverter.ToString(hash).Replace("-", "").ToLower());
        }

        private static void ExecuteClientTests()
        {
            // Case: invalid Chain
            ExecuteClientTest("063138219", "invalidchain", sectors[0].id);           
            Console.WriteLine();

            // Case: invalid Sector
            ExecuteClientTest("063138219", chains[0].id, "invalidsector");
            Console.WriteLine();

            // Cases: valid requests
            ExecuteClientTest("063138219", chains[0].id, sectors[0].id);
            ExecuteClientTest("teacher@school.com", chains[0].id, sectors[0].id);
            Console.WriteLine();
        }

        private static void ExecuteClientTest(string input, string chainGuid, string sectorGuid)
        {
            string scryptHash = GenerateScryptHash(input);
            try {
                Console.WriteLine("Generated SchoolID: " + GenerateSchoolID(scryptHash, chainGuid, sectorGuid));
            } 
            catch (Exception e)
            {
                Console.WriteLine("Exception has been thrown: " + e.Message);
            }
        }        

        private static string GenerateScryptHash(string input)
        {
            ScryptUtil scryptUtil = new ScryptUtil();

            byte[] scryptHash = scryptUtil.GenerateHash(input);
                        
            return BitConverter.ToString(scryptHash).Replace("-", "").ToLower();            
        }

        private static string GenerateSchoolID(string scryptHash, string chainGuid, string sectorGuid)
        {
            return schoolIDServiceUtil.GenerateSchoolID(scryptHash, chainGuid, sectorGuid);
        }
    }
}
