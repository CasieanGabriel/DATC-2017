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
            Session["poze"] = service.GetPoze();
            return View(Session["poze"]);
        }

        [HttpPost]
        public ActionResult IncarcaPoza(HttpPostedFileBase file)
        {
            var service = new AlbumFotoService();
            if (file!=null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }
            Session["poze"] = service.GetPoze();
            return View("Index", Session["poze"]);
        }

        [HttpPost]
        public ActionResult PosteazaComentariu(string text, string poza)
        {
            var service = new AlbumFotoService();
            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(poza))
            {
                service.PosteazaComentariu(Guid.NewGuid().ToString(), poza, text);
            }
            Session["poze"] = service.GetPoze();
            return View("Index", Session["poze"]);
        }

        public ActionResult ShareAccess(string desc)
        {
            List<Poza> poze = Session["poze"] as List<Poza>;
            ViewBag.AccessUrl = "AccessUrl : " + poze.FirstOrDefault(p => p.Description.Equals(desc)).Url;
            return View("Index", Session["poze"]);
        }
    }
}
