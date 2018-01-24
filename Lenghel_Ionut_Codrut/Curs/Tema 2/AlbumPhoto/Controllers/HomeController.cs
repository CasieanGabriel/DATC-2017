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
            if (file != null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult IncarcaComentariu(string fileName, string comment, string userName)
        {
            var service = new AlbumFotoService();
            if (comment != null && comment.Length > 0)
            {
                service.IncarcaComentariu(fileName, comment, userName);
            }

            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult ArataCometarii(string fileName, string userName, string comentariu)
        {
            var service = new AlbumFotoService();
            return View("Comentarii", service.ArataCometarii(fileName, userName, comentariu));
        }

        [HttpPost]
        public ActionResult SharedLink(string fileName, string URL)
        {
            var service = new AlbumFotoService();
            return View("Index", service.GetPoze());
        }
    }
}
