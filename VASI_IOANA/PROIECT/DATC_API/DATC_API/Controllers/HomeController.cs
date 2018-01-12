using DATC_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace DATC_API.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con;
        SqlCommand sqlCmd;

        public static List<RawData> RD_List = new List<RawData>();
        public static List<ReservationRequest> RR_List = new List<ReservationRequest>();
        public const int PL_Nr = 4;
        public const int PS_Nr = 10;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View("Index");
        }

        [HttpGet]
        public ActionResult GetRawData()
        {
            return View("RawData", RD_List);
        }

        [HttpPost]
        public ActionResult PostRawData()
        {
            int PS_Total = PL_Nr * PS_Nr;

            RawData rd = new RawData();
            rd.FreeOrNot = Request["FoN"];
            rd.ResOrOcc = Request["RoO"];
            rd.Malfunction = Request["Mal"];

            if (rd.FreeOrNot != string.Empty && rd.ResOrOcc != string.Empty && rd.Malfunction != string.Empty)
            {
                if (rd.FreeOrNot.Length == PS_Total && rd.ResOrOcc.Length == PS_Total && rd.Malfunction.Length == PS_Total)
                {
                    RD_List.Add(rd);

                    String conString = "Server=tcp:intelli.database.windows.net,1433;Initial Catalog=intellipark;Persist Security Info=False;User ID=intellipark;Password=#3intellius;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; ";
                    con = new SqlConnection(conString);
                    con.Open();

                    string query = "Insert into senzori(free, occupied_or_reserved, malfunction) values('" + rd.FreeOrNot + "','" + rd.ResOrOcc + "','" + rd.Malfunction + "')";

                    sqlCmd = new SqlCommand(query);
                    sqlCmd.CommandType = System.Data.CommandType.Text;
                    sqlCmd.Connection = con;
                    sqlCmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            
            return View("RawData", RD_List);
        }
        
        [HttpGet]
        public JsonResult GetJsonReservationRequests()
        {
            List<ReservationRequest> RR_Aux = new List<ReservationRequest>(RR_List);
            RR_List.Clear();

            //string viewContent = ConvertViewToString("ReservationRequests", RR_Aux);
            //return Json(new { PartialView = viewContent }, JsonRequestBehavior.AllowGet);

            //StringContent SC_Content = new StringContent(JsonConvert.SerializeObject(RR_Aux));
            //StringContent SC_Content = new StringContent(JsonConvert.SerializeObject(new ReservationRequest("A", "1")));
            //SC_Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return Json(JsonConvert.SerializeObject(RR_Aux), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetReservationRequests()
        {
            List<ReservationRequest> RR_Aux = new List<ReservationRequest>(RR_List);
            RR_List.Clear();
            return View("ReservationRequests", RR_Aux);
        }

        [HttpPost]
        public ActionResult PostReservationRequests()
        {
            ReservationRequest rr = new ReservationRequest();
            rr.ParkingLot = Request["PL"];
            rr.ParkingSpace = Request["PS"];

            if(rr.ParkingLot != string.Empty && rr.ParkingSpace != string.Empty)
            {
                RR_List.Add(rr);
            }

            return View("ReservationRequests", RR_List);
        }

        private string ConvertViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext vContext = new ViewContext(this.ControllerContext, vResult.View, ViewData, new TempDataDictionary(), writer);
                vResult.View.Render(vContext, writer);
                return writer.ToString();
            }
        }
    }
}
