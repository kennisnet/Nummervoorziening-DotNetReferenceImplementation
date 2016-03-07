using NVA_DotNetReferenceImplementation.SchoolID;
using NVA_DotNetReferenceImplementation.SCrypter;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ConsoleNVAClient
{
    class Program
    {
        private static SchoolIDServiceUtil schoolIDServiceUtil;
        private static Chain[] chains;
        private static Sector[] sectors;

        private static bool invalid = false;

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
            Util scryptUtil = new Util();
            byte[] hash = scryptUtil.GenerateHash("secret");
            Console.WriteLine("Base64: " + Convert.ToBase64String(hash));
            Console.WriteLine("Base16: " + BitConverter.ToString(hash).Replace("-", "").ToLower());
        }

        private static void ExecuteClientTests()
        {
            // Case: invalid BSN
            ExecuteClientTestForStudentBSN("1234567890", chains[0].id, sectors[0].id);
            Console.WriteLine();

            // Case: invalid PGN
            ExecuteClientTestForStudentPGN("1234567890", chains[0].id, sectors[0].id);
            Console.WriteLine();

            // Case: invalid Email
            ExecuteClientTestForTeacher("emailaddresswithoutatsigndotsandtld", chains[0].id, sectors[0].id);
            Console.WriteLine();

            // Case: invalid Chain
            ExecuteClientTestForStudentBSN("063138219", "invalidchain", sectors[0].id);
            ExecuteClientTestForStudentPGN("063138219", "invalidchain", sectors[0].id);
            ExecuteClientTestForTeacher("teacher@school.com", "invalidchain", sectors[0].id);
            Console.WriteLine();

            // Case: invalid Sector
            ExecuteClientTestForStudentBSN("063138219", chains[0].id, "invalidsector");
            ExecuteClientTestForStudentPGN("063138219", chains[0].id, "invalidsector");
            ExecuteClientTestForTeacher("teacher@school.com", chains[0].id, "invalidsector");
            Console.WriteLine();

            // Case: valid request
            ExecuteClientTestForStudentBSN("063138219", chains[0].id, sectors[0].id);
            ExecuteClientTestForStudentPGN("063138219", chains[0].id, sectors[0].id);
            ExecuteClientTestForTeacher("teacher@school.com", chains[0].id, sectors[0].id);
            Console.WriteLine();
        }

        private static void ExecuteClientTestForStudentBSN(string bsn, string chainGuid, string sectorGuid)
        {
            if (IsValidBsn(bsn))
            {
                string scryptHash = GenerateScryptHash(bsn);
                Console.WriteLine("Generated SchoolID: " + GenerateSchoolID(scryptHash, chainGuid, sectorGuid));
            } else
            {
                Console.WriteLine("Input is not a valid BSN: " + bsn);
            }
        }

        private static void ExecuteClientTestForStudentPGN(string pgn, string chainGuid, string sectorGuid)
        {
            if (IsValidPgn(pgn))
            {
                string scryptHash = GenerateScryptHash(pgn);
                Console.WriteLine("Generated SchoolID: " + GenerateSchoolID(scryptHash, chainGuid, sectorGuid));
            }
            else
            {
                Console.WriteLine("Input is not a valid PGN: " + pgn);
            }
        }

        private static void ExecuteClientTestForTeacher(string email, string chainGuid, string sectorGuid)
        {
            if (IsValidEmail(email))
            {
                string scryptHash = GenerateScryptHash(email);
                Console.WriteLine("Generated SchoolID: " + GenerateSchoolID(scryptHash, chainGuid, sectorGuid));
            } else
            {
                Console.WriteLine("Input is not a valid E-mail address: " + email);
            }
        }

        private static bool IsValidBsn(string bsn)
        {
            int divisor = 1000000000;
            int total = 0;
            int result = Int32.Parse(bsn);

            for (int i = 9; i > 1; i--)
            {
                total += i * Math.DivRem(result, divisor /= 10, out result);
            }

            int rest;
            Math.DivRem(total, 11, out rest);

            return result == rest;
        }

        private static bool IsValidPgn(string bsn)
        {
            if (string.IsNullOrEmpty(bsn))
            {
                return false;
            }

            int divisor = 1000000000;
            int total = 0;
            int result = Int32.Parse(bsn);

            for (int i = 9; i > 1; i--)
            {
                total += i * Math.DivRem(result, divisor /= 10, out result);
            }

            int rest;
            Math.DivRem(total, 11, out rest);

            return result == rest;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            invalid = false;
            if (String.IsNullOrEmpty(email))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(email,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        private static string GenerateScryptHash(string input)
        {
            Util scryptUtil = new Util();

            byte[] scryptHash = scryptUtil.GenerateHash(input);
                        
            return BitConverter.ToString(scryptHash).Replace("-", "").ToLower();            
        }

        private static string GenerateSchoolID(string scryptHash, string chainGuid, string sectorGuid)
        {
            // STUB: Should invoke School ID 
            return scryptHash;
        }
    }
}
