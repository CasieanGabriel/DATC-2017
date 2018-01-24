using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlbumPhoto.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var service = new AlbumFotoService();
            return View(service.GetPoze());
        }
        public ActionResult vezicomentari(string pictureName)
        {
            var service = new AlbumFotoService();
            return View(service.vezicomentarile(pictureName));
        }
        [HttpPost]
        public ActionResult IncarcaPoza(HttpPostedFileBase file)
        {
            var service = new AlbumFotoService();
            if (file!=null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.GetPoze());
        }
        public ActionResult comentari(string pictureName)
        {
            return View();
        }
        public ActionResult link(string pictureName)
        {
            return View();
        }
    }
}
