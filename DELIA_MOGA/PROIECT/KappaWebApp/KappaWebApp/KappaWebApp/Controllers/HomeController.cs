using KappaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace KappaWebApp.Controllers
{
    public class HomeController : Controller
    {
        public static string _connectionString = "Server=tcp:kappaserver.database.windows.net;Database=kappa_database;User ID =IonutGrad;Password=GradIonut1;Trusted_Connection=False;Encrypt=True;";
        public ActionResult Index()
        {
            List<SensorData> sd = new List<SensorData>();
            SqlConnection dbconn = new SqlConnection(_connectionString);
            try
            {
                dbconn.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Zone", dbconn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sd.Add(new SensorData {
                         //   Humidity = Convert.ToDouble(reader["Humidity"]),
                        //    Temperature = Convert.ToDouble(reader["Temperature"]),
                            Lat = Convert.ToDouble(reader["Latitude"].ToString()),
                            Lng = Convert.ToDouble(reader["Longitude"].ToString()),
                        //    Data = Convert.ToDateTime(reader["Data"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
                var a = exp.Message.ToString();
            }
            

            return View(sd);
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