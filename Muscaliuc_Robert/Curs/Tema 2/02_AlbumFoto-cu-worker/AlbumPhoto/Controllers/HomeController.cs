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
        public ActionResult AdaugaComentariu()
        {
            string comentariu = Request.Form["comentariu"];
            string numePoza = Request.Form["numePoza"];
            var service = new AlbumFotoService();

            string text = comentariu + "+" + numePoza;
            if (comentariu!= null)
            {
                service.AdaugaComentariu("guest", text);
            }
            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult VeziComentarii()
        {
            var service = new AlbumFotoService();
            string numePoza = Request.Form["numePoza"];
            return View("Comentarii", service.GetComments(numePoza));
        }

        [HttpPost]
        public ActionResult VeziLinkAcces()
        {
            var service = new AlbumFotoService();
            string numePoza = Request.Form["numePoza"];
            return View("LinkAcces",null, service.GetBlobSasUri(numePoza));
        }
    }
}
