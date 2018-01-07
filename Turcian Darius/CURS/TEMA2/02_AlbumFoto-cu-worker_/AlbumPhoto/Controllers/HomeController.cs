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

        [HttpPost]
        public ActionResult IncarcaComentariu(string comment, string description, string madeBy)
        {
            var service = new AlbumFotoService();
            if (!string.IsNullOrEmpty(comment))
            {
                service.IncarcaComentariu(madeBy, description, comment);
            }

            return View("Index", service.GetPoze());
        }

        [HttpGet]
        public ActionResult GetAccessLink(string fileName)
        {
            var service = new AlbumFotoService();
            var link = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                link = service.GetAcceessLink(fileName);
            }

            ViewBag.link = link;
            ViewBag.description = fileName;

            return View("Index", service.GetPoze());
        }
    }
}
