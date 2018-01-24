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
        public ActionResult IncarcaComm(HttpPostedFileBase file)
        {
            var service = new AlbumFotoService();
            var photo = Request["Poza"].ToString();
            var comm = Request["Comm"].ToString();
            var user = Request["User"].ToString();
            service.IncarcaComm(user, photo, comm);
            return View("Index", service.GetPoze());

        }

        [HttpPost]
        public ActionResult GenerareLink(string poza)
        {
            var service = new AlbumFotoService();
            return View("GenerareLink", service.GenerareLink(poza));
        }
    }
}
