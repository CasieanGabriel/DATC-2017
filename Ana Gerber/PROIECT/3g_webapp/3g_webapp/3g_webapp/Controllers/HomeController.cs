using 3G_WebApp.Models;
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

namespace 3G_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public static string _connectionString = "Server=tcp:3g-server.database.windows.net,1433;Initial Catalog = 3G-DB;Persist Security Info=False;User ID = AnaGerber; Password=Angerb48!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";
        public ActionResult VizualizeHumidity()
        {
            IEnumerable<Date> Idate = null;
            Idate = GetDataFromCloud("http://api20180109081124.azurewebsites.net/api/values/");
           // Idate = GetDataFromCloud("http://localhost:55006/api/values");

            //string newurl = "http://localhost:55006/api/values/";
            return View(Idate);
        }

        public ActionResult VizualizeTemperature()
        {
            IEnumerable<Date> Idate = null;
            Idate = GetDataFromCloud("http://api20180109081124.azurewebsites.net/api/values/");
           // Idate = GetDataFromCloud("http://localhost:55006/api/values/");

            return View(Idate);
        }

        public ActionResult VizualizareIstoric(string Latitude, string Longitude)
        {
            IEnumerable<Date> Idate = null;
            Idate = GetDataFromCloud("http://api20180109081124.azurewebsites.net/api/values/?Latitudine=" + Latitude + "&Longitudine=" + Longitude);
            //Idate = GetDataFromCloud("http://localhost:55006/api/values/5?Latitudine=" + Latitude + "&Longitudine=" + Longitude);

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

        public static IEnumerable<Date> GetDataFromCloud(string URL)
        {
            IEnumerable<Date> Idate = null;
            try
            {

                string newurl = "http://api20180109081124.azurewebsites.net/api/values/";
                //string newurl = "http://localhost:55006/api/values";
                var request = WebRequest.Create(URL);
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
            {
                exp.Message.ToString();
            }
            return Idate;
        }
    }
}