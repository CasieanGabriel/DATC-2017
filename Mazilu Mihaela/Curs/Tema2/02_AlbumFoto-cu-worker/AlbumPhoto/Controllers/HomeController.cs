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

        public ActionResult IntroduComm(string comentariu, string poza)
        {
            var service = new AlbumFotoService();

            if (!(string.IsNullOrEmpty(comentariu)) && !(string.IsNullOrEmpty(poza)))
            {
                service.IntroduComm(Guid.NewGuid().ToString(), poza, comentariu);
            }
            return View("Index", service.GetPoze());
        }

        public ActionResult VizualizareComm()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Index", service.GetComm(poza)); 
        }

        public ActionResult GenLink()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("LINK", service.GenLink(poza));
        }
    }
}
