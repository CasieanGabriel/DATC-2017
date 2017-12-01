using AlbumPhoto.Models;
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
        public ActionResult AdaugaComentariu(string Text1, string poza, string madeBy)
        {
            var service = new AlbumFotoService();
            if (Text1 != null && poza != null)
            {
                service.AdaugaComentariu(madeBy != "" ? madeBy : "guest", Text1, poza);
            }

            return View("Index", service.GetPoze());
        }

        [HttpGet]
        public ActionResult GetComentarii(Poza poza)
        {
            var service = new AlbumFotoService();
            return View("Comentarii", service.GetComentarii(poza.Description));
        }

        [HttpGet]
        public ActionResult GetLink(Poza poza)
        {
            var service = new AlbumFotoService();

            ViewBag.Link = AlbumFotoService.GetBlobSasUri(service.PhotoContainer, poza.Description);

            return View("Index", service.GetPoze());
        }
    }
}
