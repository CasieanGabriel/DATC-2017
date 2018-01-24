using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlbumPhoto.Models;

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
        public ActionResult AdaugaComentariu(string numeUtilizator, string comentariu, string poza)
        {
            var service = new AlbumFotoService();
            if (comentariu != "" && poza != "")
            {
                service.AdaugaComentariu(numeUtilizator, comentariu, poza);
            }
            return View("Index", service.GetPoze());
        }

        [HttpGet]
        public ActionResult VizualizareComentarii(string poza)
        {
            var service = new AlbumFotoService();
            return View("VizualizareComentarii", service.VizualizareComentarii(poza));
        }
        
        [HttpPost]
        public ActionResult GenerareLink(string poza)
        {
            var service = new AlbumFotoService();
            return View("GenerareLink", service.GenerareLink(poza));
        }
    }
}
