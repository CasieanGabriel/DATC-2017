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
        public ActionResult AdaugaComentariu(string rowKey, string autor, string text)
        {
            var service = new AlbumFotoService();
            if (!string.IsNullOrEmpty(autor) && !string.IsNullOrEmpty(text))
            {
                var comentariu = new Comentariu()
                {
                    Autor = autor,
                    Text = text
                };

                service.AdaugaComentariu(comentariu, rowKey);
            }

            return View("Index", service.GetPoze());
        }

        public ActionResult GenerareLink(string url)
        {
            var service = new AlbumFotoService();
            var link = service.GenerateLink(url);

            return View("GenerareLink", (object)link);
        }
    }
}
