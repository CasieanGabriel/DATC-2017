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
        public ActionResult GetComentariu()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Comentarii",service.GetComentarii(poza));
        }

        [HttpPost]
        public ActionResult GetLink()
        {
            var service = new AlbumFotoService();
            var poza = Request["Picture"].ToString();
            return View("Link", service.GetLink(poza));
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
            var service = new AlbumFotoService();
            var by = Request["By"].ToString();
            var poza = Request["Picture"].ToString();
            if (Request["Comentariu"].ToString().Length>0 && Request["Comentariu"].ToString()!=null)
            {
                var txtComm = Request["Comentariu"].ToString();
                txtComm = poza + "#%#" + txtComm;
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(by+": "+txtComm);
                writer.Flush();
                stream.Position = 0;
                service.IncarcaComentariu("guest", txtComm, by,stream);
            }
            return View("Index", service.GetPoze());
        }
    }
}
