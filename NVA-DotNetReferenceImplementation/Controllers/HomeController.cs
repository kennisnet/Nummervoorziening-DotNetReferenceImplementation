using System.Web.Mvc;
using CryptSharp.Utility;
using System.Text;
using System;

namespace NVA_DotNetReferenceImplementation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            byte[] hash = Hash("secret", "rktYml0MIp9TC9u6Ny6uqw==");
            
            ViewBag.Base64Hash = Convert.ToBase64String(hash);
            ViewBag.HexHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return View();
        }

        private byte[] Hash(string password, string salt)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);
            int N = 16384;
            int r = 8;
            int p = 1;
            int? maxThreads = (int?)null;
            int derivedKeyLength = 32;
            
            return SCrypt.ComputeDerivedKey(keyBytes, saltBytes, N, r, p, maxThreads, derivedKeyLength);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}