#region License
/*
Copyright 2016, Stichting Kennisnet

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

namespace ConsoleNVAClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using NVA_DotNetReferenceImplementation.SchoolID;
    using NVA_DotNetReferenceImplementation.SCrypter;

    /// <summary>
    /// The main entry point for the program. This function demonstrates work with Web Services via SchoolID project.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Object to store the proxy class which is used to communicate with the Nummervoorziening service
        /// </summary>
        private static SchoolIDServiceUtil schoolIDServiceUtil;

        /// <summary>
        /// Chains retrieved from the Nummervoorziening service
        /// </summary>
        private static Chain[] chains;

        /// <summary>
        /// Sectors retrieved from the Nummervoorziening service
        /// </summary>
        private static Sector[] sectors;

        /// <summary>
        /// An example PGN of a student
        /// </summary>
        private static string studentPgn = "063138219";

        /// <summary>
        /// An example PGN of a teacher
        /// </summary>
        private static string teacherPgn = "20DP teacher@school.com";

        /// <summary>
        /// Console application entrance of the Reference implementation to demonstrate how a Nummervoorziening client may communicate 
        /// with the Nummervoorziening service.
        /// </summary>
        /// <param name="args">Optional parameters (not used)</param>
        public static void Main(string[] args)
        {
            // Disable SSL checks for now
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;
            
            // Setup the Service Utility for School ID 
            schoolIDServiceUtil = SchoolIDServiceUtil.Instance;

            Console.WriteLine("Current server information:");

            try
            {
                // Status information
                if (schoolIDServiceUtil.IsSchoolIDAvailable())
                {
                    // Print some information about the service
                    WritePingStatusOutput();

                    // List available chains
                    WriteAvailableChains();

                    // List available sectors
                    WriteAvailableSectors();

                    // Retrieve a Stampseudonym
                    Console.WriteLine("\nRetrieving Stampseudonym:");
                    Console.WriteLine("Pgn:\t\t\t\t" + studentPgn);
                    string studentHpgn = GenerateScryptHash(studentPgn);
                    Console.WriteLine("HPgn:\t\t\t\t" + studentHpgn);
                    string studentStampseudonym = ExecuteCreateStampseudonymTest(studentHpgn);
                    Console.WriteLine("Retrieved Stampseudonym:\t" + studentStampseudonym + "\n");
                    Console.WriteLine("Pgn:\t\t\t\t" + teacherPgn);
                    string teacherHpgn = GenerateScryptHash(teacherPgn);
                    Console.WriteLine("HPgn:\t\t\t\t" + teacherHpgn);
                    string teacherStampseudonym = ExecuteCreateStampseudonymTest(teacherHpgn);
                    Console.WriteLine("Retrieved Stampseudonym:\t" + teacherStampseudonym + "\n");

                    // Execute a batch operation for retrieving Stampseudonyms
                    Dictionary<int, string> listedHpgnDictionary = new Dictionary<int, string>();
                    listedHpgnDictionary.Add(0, studentHpgn);
                    listedHpgnDictionary.Add(1, teacherHpgn);

                    Console.WriteLine("Submitting Stampseudonym batch (with the same input)");
                    ExecuteStampseudonymBatchTest(listedHpgnDictionary);

                    // Retrieve a SchoolID
                    Console.WriteLine("\nRetrieving SchoolID for first active sector and first active chain:");
                    Console.WriteLine("Chain Guid:\t\t\t" + chains[0].id);
                    Console.WriteLine("Sector Guid:\t\t\t" + sectors[0].id);
                    
                    ExecuteCreateEckIdTest(studentStampseudonym, chains[0].id, sectors[0].id);
                    ExecuteCreateEckIdTest(teacherStampseudonym, chains[0].id, sectors[0].id);
                }
                else
                {
                    Console.WriteLine("School ID service is offline or you are not authorized to use it.");
                }
            }
            catch (FaultException fe)
            {
                Console.WriteLine("Fault received: " + fe.Message);
            }
            catch (EndpointNotFoundException enfe)
            {
                Console.WriteLine("Configured Endpoint not found: " + enfe.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to quit");
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
        /// Executes tests for retrieving Stampseudonym based on PGN
        /// </summary>
        /// <param name="hpgn">The Pgn that is hashed and send to Nummervoorziening</param>
        /// <returns>The Stampseudonym for the HPgn</returns>
        private static string ExecuteCreateStampseudonymTest(string hpgn)
        {
            // Retrieve Stampseudonym from Nummervoorziening service
            return schoolIDServiceUtil.GenerateStampseudonym(hpgn);
        }

        /// <summary>
        /// Executes tests submitting and retrieving Stampseudonym batch
        /// </summary>
        /// <param name="listedHpgnDictionary">An indexed list of HPGNs</param>
        private static void ExecuteStampseudonymBatchTest(Dictionary<int, string> listedHpgnDictionary)
        {
            try
            {
                string batchIdentifier = schoolIDServiceUtil.SubmitStampseudonymBatch(listedHpgnDictionary);
                Console.WriteLine("Batch identifier:\t\t" + batchIdentifier);
                Console.WriteLine("Waiting for processing...");
                SchoolIDBatch stampseudonymBatch = schoolIDServiceUtil.RetrieveBatch(batchIdentifier);

                if (stampseudonymBatch != null)
                {
                    string successList = stampseudonymBatch.getSuccessList().Count > 0
                                             ? stampseudonymBatch.getSuccessList()
                                                 .Select(x => x.Key + "=" + x.Value)
                                                 .Aggregate((s1, s2) => s1 + ", " + s2)
                                             : string.Empty;
                    string failedList = stampseudonymBatch.getFailedList().Count > 0
                                             ? stampseudonymBatch.getSuccessList()
                                                 .Select(x => x.Key + "=" + x.Value)
                                                 .Aggregate((s1, s2) => s1 + ", " + s2)
                                             : string.Empty;
                    Console.WriteLine("Generated Stampseudonyms:\t{" + successList + "}");
                    Console.WriteLine("Failed Stampseudonyms:\t{" + failedList + "}");
                }
                else
                {
                    Console.WriteLine("Error occured: StampseudonymBatch is null.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception has been thrown: " + e.Message);
            }
        }

        /// <summary>
        /// Executes test cases
        /// </summary>
        /// <param name="stampseudonym">The Stampseudonym input</param>
        /// <param name="chainGuid">A valid Chain Guid</param>
        /// <param name="sectorGuid">A valid Sector Guid</param>
        private static void ExecuteCreateEckIdTest(string stampseudonym, string chainGuid, string sectorGuid)
        {
            try
            {
                Console.WriteLine("Retrieved SchoolID:\t\t" + GenerateSchoolID(stampseudonym, chainGuid, sectorGuid));
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
            return scryptUtil.GenerateHexHash(input);
        }
        
        /// <summary>
        /// Wrapper function to retrieve a SchoolID
        /// </summary>
        /// <param name="hpgn">A scrypt hash of a PGN</param>
        /// <param name="chainGuid">A valid Chain Guid</param>
        /// <param name="sectorGuid">A valid Sector Guid</param>
        /// <returns>The generated Stampseudonym</returns>
        private static string GenerateSchoolID(string hpgn, string chainGuid, string sectorGuid)
        {
            return schoolIDServiceUtil.GenerateSchoolID(hpgn, chainGuid, sectorGuid);
        }
    }
}
