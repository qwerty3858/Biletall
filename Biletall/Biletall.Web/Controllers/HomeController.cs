using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Biletall.Web.Models;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Server.Kestrel;
using System.ServiceModel.Channels;
using System.ServiceModel;
using ServiceReference1;
using Biletall.Web.Controllers;

namespace Biletall.Web.Controllers
{
    public class KaraNokta
    {
        public string ID { get; set; }
        public string SeyahatSehirID { get; set; }
        public string Bolge { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public string MerkezMi { get; set; }
        public string BagliOlduguNoktaID { get; set; }

        }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       

        public IActionResult Index()
        {

            XmlIsletRequestBody xirb = new XmlIsletRequestBody();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml("<Kullanici><Adi>" + "stajyerWS" + "</Adi><Sifre>" + "2324423WSs099"
+ "</Sifre></Kullanici>");
            xirb.xmlYetki = xml.DocumentElement;

            XmlDocument xml2 = new XmlDocument();
            xml2.LoadXml(@"<KaraNoktaGetirKomut/>");
            xirb.xmlIslem = xml2.DocumentElement;

            var xx = new XmlIsletRequest(xirb);

            var service = new ServiceSoapClient(ServiceSoapClient.EndpointConfiguration.ServiceSoap).XmlIslet(xirb.xmlIslem, xirb.xmlYetki);

            List<KaraNokta> list = new List<KaraNokta>();

            XmlNodeList xnList = service.SelectNodes("/KaraNokta");
            foreach (XmlNode xn in xnList)
            {
                KaraNokta kn = new KaraNokta
                {
                    ID = xn["ID"].InnerText,
                    Ad = xn["Ad"].InnerText,
                    Aciklama = xn["Aciklama"].InnerText,
                    BagliOlduguNoktaID = xn["BagliOlduguNoktaID"].InnerText,
                    Bolge = xn["Bolge"].InnerText,
                    MerkezMi = xn["MerkezMi"].InnerText,
                    SeyahatSehirID = xn["SeyahatSehirID"].InnerText
                };
                if (kn.MerkezMi == "1")
                {
                    list.Add(kn);
                }
            }
            //return data;
            ViewBag.Cities = list;

            return View();
        }


        [HttpPost]
        public IActionResult Index(string nereden, string nereye, string tarih)
        {

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
