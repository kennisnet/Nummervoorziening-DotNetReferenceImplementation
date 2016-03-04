using System.Web.Mvc;
using CryptSharp.Utility;
using System.Text;
using System;
using NVA_DotNetReferenceImplementation.SCrypter;

namespace NVA_DotNetReferenceImplementation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Util scryptUtil = new Util();

            byte[] hash = scryptUtil.GenerateHash("secret");

            System.Net.ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) => true);

            ViewBag.Base64Hash = Convert.ToBase64String(hash);
            ViewBag.HexHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

            SchoolIDClient schoolID = new SchoolIDClient();

            PingRequest pingRequest = new PingRequest();
            pingRequest1 requestWrapper = new pingRequest1();

            requestWrapper.pingRequest = pingRequest;

            pingResponse1 response = schoolID.ping(requestWrapper);

            PingResponse pingResponse = response.pingResponse;

            return View();
        }        
    }
}