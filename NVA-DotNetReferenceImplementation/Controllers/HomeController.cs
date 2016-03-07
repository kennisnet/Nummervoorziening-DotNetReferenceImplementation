using System.Web.Mvc;
using CryptSharp.Utility;
using System.Text;
using System;
using NVA_DotNetReferenceImplementation.SCrypter;
using NVA_DotNetReferenceImplementation.SchoolID;
using System.Collections.Generic;

namespace NVA_DotNetReferenceImplementation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Disable SSL checks for now
            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            // Setup the Service Utility for School ID 
            SchoolIDServiceUtil schoolIDServiceUtil = SchoolIDServiceUtil.Instance;
            
            // Status information
            ViewBag.IsOnline = schoolIDServiceUtil.isSchoolIDAvailable();
            ViewBag.DateTimeOnServer = schoolIDServiceUtil.getSchoolIDDateTime();
            ViewBag.SchoolIDVersion = schoolIDServiceUtil.getSchoolIDVersion();

            // Retrieve Chains
            List<string> chains = new List<string>();            
            foreach (Chain chain in schoolIDServiceUtil.getChains())
            {
                chains.Add(chain.name + " (" + chain.id + "): " + chain.description);
            }
            ViewBag.Chains = chains.ToArray();

            // Retrieve Sectors
            List<string> sectors = new List<string>();
            foreach (Sector sector in schoolIDServiceUtil.getSectors())
            {
                sectors.Add(sector.name + " (" + sector.id + "): " + sector.description);
            }
            ViewBag.Sectors = sectors.ToArray();





            // Some scrypt tests
            Util scryptUtil = new Util();
            byte[] hash = scryptUtil.GenerateHash("secret");
            ViewBag.Base64Hash = Convert.ToBase64String(hash);
            ViewBag.HexHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return View();
        }        
    }
}