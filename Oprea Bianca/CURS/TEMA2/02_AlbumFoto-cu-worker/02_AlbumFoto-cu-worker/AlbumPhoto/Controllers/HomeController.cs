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
        public ActionResult VizualizeazaComentarii(string descriere_poza)
        {
            var service = new AlbumFotoService();
            descriere_poza = Request["Descriere"].ToString();
            return View("Comentarii", service.GetCommentarii(descriere_poza));
        }

        [HttpPost]
        public ActionResult AdaugaComentarii()
        {
            var service = new AlbumFotoService();
            string comentarii = Request["Comentarii"].ToString();
            string utilizator = Request["Utilizator"].ToString();
            string descriere = Request["Descriere"].ToString();
            if (!string.IsNullOrEmpty(utilizator) && !string.IsNullOrEmpty(utilizator))

            {
            service.AdaugaComentarii(descriere+comentarii, utilizator, descriere);
            }
            return View("Index", service.GetPoze());
        }

        public ActionResult LinkAcces(string descriere_poza)
        {
            var service = new AlbumFotoService();
            return View();
        }
    }
}
