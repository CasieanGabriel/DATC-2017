using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

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
            if (file != null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.GetPoze());
        }


        [HttpPost]
        public ActionResult GetComentariu(string picture) // == ViewComments
        {
            var service = new AlbumFotoService();
            return View("Comentarii",service.AfisareComentariu(picture));
        }

        [HttpPost]
        public ActionResult AdaugaComentarii()
        {
            var service = new AlbumFotoService();
            string userName = Request["User"].ToString();
            string commment = Request["Comentariu"].ToString();
            string picture = Request["Picture"].ToString();

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(commment))
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(userName + ":" + commment);
                writer.Flush();
                stream.Position = 0;
                service.AddComment(userName, commment, picture,stream);
            }
            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult LinkPoza()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Link", service.LinkPoza(poza));
        }
    }
}
