using KappaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace KappaWebApp.Controllers
{
    public class HomeController : Controller
    {
        public static string _connectionString = "Server=tcp:iogrserver.database.windows.net,1433;Initial Catalog=IoGrDatabase;Persist Security Info=False;User ID=IonutGrad;Password=GradIonut1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public ActionResult VizualizeHumidity()
        {
            IEnumerable<Date> Idate = null;
            Idate = GetDataFromCloud();
            return View(Idate);
        }

        public ActionResult VizualizeTemperature()
        {
            IEnumerable<Date> Idate = null;
            Idate = GetDataFromCloud();
            return View(Idate);
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

        public static IEnumerable<Date> GetDataFromCloud()
        {
            IEnumerable<Date> Idate = null;
            try
            {

                string newurl = "http://kappaapi.azurewebsites.net/api/values/";
                var request = WebRequest.Create(newurl);
                string text = string.Empty;
                request.ContentType = "application/json";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }
                }
                Idate = JsonConvert.DeserializeObject<IEnumerable<Date>>(text);
                //  Console.WriteLine(jsonConvertedData);
            }
            catch (Exception exp)
            { }
            return Idate;
        }
    }
}