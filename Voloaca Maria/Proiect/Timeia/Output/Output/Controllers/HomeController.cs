using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Output.Models;
using System.Data.Entity;
namespace Output.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (var db = new ParkingContext())
            {
                // Create and save a new parking lot
                Console.Write("Enter a name for a new Lot: ");
                var coord = Console.ReadLine();

                var pLot = new ParkingLot { Coordinates = coord };
                db.ParkingLots.Add(pLot);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.ParkingLots
                            orderby b.Coordinates
                            select b;

                Console.WriteLine("All lots in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.LotId);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            return View();

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
