using AlbumPhoto.Service;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult GetComentariu()
        {
            var service = new AlbumFotoService();
            string poza = Request["Picture"].ToString();
            return View("Comentarii", service.GetComment(poza));
        }

        [HttpPost]
        public ActionResult GetLink()
        {
            var service = new AlbumFotoService();
            string poza = Request["Picture"].ToString();
            return View("Link", service.GetLink(poza));
        }

        [HttpPost]
        public ActionResult AdaugaComentariu()
        {
            var service = new AlbumFotoService();
            var by = Request["By"].ToString();
            string poza = Request["Picture"].ToString();
            string comment = Request["Comentariu"].ToString();
            if (comment.Length > 0 && comment != null)
            {
                comment = poza + "#%#" + comment;
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(by + ": " + comment);
                writer.Flush();
                stream.Position = 0;
                service.AddComment("guest", comment, by, stream);
            }
            return View("Index", service.GetPoze());
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
    }
}
