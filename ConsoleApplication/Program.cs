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
        
        /// <summary>
        /// Console application entrance of the Reference implementation to demonstrate how a Nummervoorziening client may communicate 
        /// with the Nummervoorziening service.
        /// </summary>
        /// <param name="args">Optional parameters (not used)</param>
        static void Main(string[] args)
        {
            // Disable SSL checks for now
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            // Setup the Service Utility for School ID 
            schoolIDServiceUtil = SchoolIDServiceUtil.Instance;

            Console.WriteLine("Current server information:");

            // Status information
            if (schoolIDServiceUtil.IsSchoolIDAvailable())
            {
                WritePingStatusOutput();               
                WriteAvailableChains();
                WriteAvailableSectors();

                // Tests               
                Console.WriteLine(); 
                ExecuteClientTests();                
            } else
            {
                Console.WriteLine("School ID service is offline.");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Displays status information regarding the Nummervoorziening service
        /// </summary>
        private static void WritePingStatusOutput()
        {
            Console.WriteLine("Application version:\t\t" + schoolIDServiceUtil.GetSchoolIDVersion());
            Console.WriteLine("System time:\t\t\t" + schoolIDServiceUtil.GetSchoolIDDateTime());
            Console.WriteLine("Available:\t\t\ttrue");
        }

        /// <summary>
        /// Displays the available active chains
        /// </summary>
        private static void WriteAvailableChains()
        {
            chains = schoolIDServiceUtil.GetChains();

            // List available Chains            
            Console.WriteLine("Count of active chains:\t\t" + chains.Length);
        }

        /// <summary>
        /// Displays the available active sectors
        /// </summary>
        private static void WriteAvailableSectors()
        {
            sectors = schoolIDServiceUtil.GetSectors();

            // List available Sectors
            Console.WriteLine("Count of active sectors:\t" + sectors.Length);
        }
        
        /// <summary>
        /// Initializes test cases
        /// </summary>
        private static void ExecuteClientTests()
        {
            Console.WriteLine("Retrieving SchoolID for first active sector and first active chain:");
            Console.WriteLine("ChainId:\t\t\t" + chains[0].id);
            Console.WriteLine("SectorId:\t\t\t" + sectors[0].id);

            // Cases: valid requests
            Console.WriteLine();
            ExecuteClientTest("063138219", chains[0].id, sectors[0].id);
            ExecuteClientTest("teacher@school.com", chains[0].id, sectors[0].id);
            Console.WriteLine();
        }

        /// <summary>
        /// Executes test cases
        /// </summary>
        /// <param name="input">The PGN input</param>
        /// <param name="chainGuid">A valid Chain Guid</param>
        /// <param name="sectorGuid">A valid Sector Guid</param>
        private static void ExecuteClientTest(string input, string chainGuid, string sectorGuid)
        {
            string scryptHash = GenerateScryptHash(input);
            try {
                Console.WriteLine("Pgn:\t\t\t\t" + input);
                Console.WriteLine("HPgn:\t\t\t\t" + scryptHash);
                Console.WriteLine("Retrieved SchoolID:\t\t" + GenerateSchoolID(scryptHash, chainGuid, sectorGuid));
                Console.WriteLine();
            } 
            catch (Exception e)
            {
                Console.WriteLine("Exception has been thrown: " + e.Message);
            }
        }        

        /// <summary>
        /// Uses the scrypt library to provide a hexadecimal scrypt hash of the input
        /// </summary>
        /// <param name="input">The input for the hash</param>
        /// <returns>A scrypt hash in hexadecimal notation</returns>
        private static string GenerateScryptHash(string input)
        {
            ScryptUtil scryptUtil = new ScryptUtil();

            // Get the hash from the scrypt library
            byte[] scryptHash = scryptUtil.GenerateHash(input);
                        
            // Return the hash in hexadecimal notation
            return BitConverter.ToString(scryptHash).Replace("-", "").ToLower();            
        }

        /// <summary>
        /// Wrapper function to retrieve a SchoolID
        /// </summary>
        /// <param name="scryptHash">A scrypt hash of a PGN</param>
        /// <param name="chainGuid">A valid Chain Guid</param>
        /// <param name="sectorGuid">A valid Sector Guid</param>
        /// <returns></returns>
        private static string GenerateSchoolID(string scryptHash, string chainGuid, string sectorGuid)
        {
            return schoolIDServiceUtil.GenerateSchoolID(scryptHash, chainGuid, sectorGuid);
        }
    }
}
