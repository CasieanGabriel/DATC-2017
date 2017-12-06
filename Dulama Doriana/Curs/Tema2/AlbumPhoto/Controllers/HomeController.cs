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
        public ActionResult AdaugaComentariu(string user, string image ,string comment)
        {
            var service = new AlbumFotoService();
            if(string.IsNullOrEmpty(user)==false && string.IsNullOrEmpty(image)==false && string.IsNullOrEmpty(comment)==false)
            {
                service.AdaugaComentariu(user, image, comment);
            }

            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult VizualizeazaComentarii(string image)
        {
            var service = new AlbumFotoService();
            return View("VizualizeazaComentarii", service.VizualizeazaComentarii(image));
        }
    }
}
