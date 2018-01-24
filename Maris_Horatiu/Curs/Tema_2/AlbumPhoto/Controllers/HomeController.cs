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
        static string Poza = "";
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
        public ActionResult GetCom()
        {
            var service = new AlbumFotoService();
            var poza = Request["Poza"].ToString();
            Poza = poza;
            return View("Comments", service.GetCom(poza));
        }

        [HttpPost]
        public ActionResult AdaugaCom()
        {
            var service = new AlbumFotoService();
            var author = Request["Author"].ToString();
            var text = Request["Comment"].ToString();
            if (author != null && text != null)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(author + " " + text);
                writer.Flush();
                stream.Position = 0;
                service.IncarcaCom(author, text, Poza, stream);
            }
            return View("Comments", service.GetCom(Poza));
        }
        
        public ActionResult GetLink()
        {
            var service = new AlbumFotoService();
            var poza = Request["Poza"].ToString();
            return View("Link", service.GetLink(poza));
        }
    }
}
