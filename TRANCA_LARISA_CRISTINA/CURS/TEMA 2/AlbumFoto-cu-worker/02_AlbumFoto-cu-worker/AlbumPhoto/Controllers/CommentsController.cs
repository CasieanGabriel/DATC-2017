using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlbumPhoto.Controllers
{
    public class CommentsController : Controller
    {
        //
        // GET: /Comments/

        public ActionResult Index()
        {
            var service = new AlbumFotoService();
            return View(service.GetPoze());
        }

        [HttpPost]
        public ActionResult AdaugaComentariu(string comentariu, string username, string poza)
        {
            var service = new AlbumFotoService();
            if (comentariu != null && comentariu.Length > 0)
            {
                service.AdaugaComentariu(comentariu,username, poza);
            }

            return View("Index", service.GetPoze());
        }

        [HttpGet]
        public ActionResult getComments(string poza)
        {
            var service = new AlbumFotoService();
            return View("Comments", service.getComments(poza));
        }

        [HttpPost]
        public ActionResult GenerareLink(string poza)
        {
            var service = new AlbumFotoService();
            return View("GenerareLink", service.GenerareLink(poza));
        }
    }
}
