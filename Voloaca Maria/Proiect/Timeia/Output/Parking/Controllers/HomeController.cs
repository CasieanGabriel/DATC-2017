using Output.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parking.Controllers
{
    public class HomeController : Controller
    {
       // public ParkingContext db = new ParkingContext();
        public ActionResult Index()
        {
            return View();
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
        [HttpGet]
        public ActionResult GetParkingLotsInfo()
        {
            using (var db = new ParkingContext())
            {
                return Json(db.ParkingLots.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult SetStatus(int id, bool status)
        {
            using (var db = new ParkingContext())
            {
                var parkingLot = db.ParkingLots.Find(id);

                if (parkingLot == null)
                {
                    Console.WriteLine("blabla");
                }
                else
                {
                    parkingLot.Status = status;
                    db.Entry(parkingLot).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }
       

    }
}