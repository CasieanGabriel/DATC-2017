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
        public ActionResult AdaugaComentariu(){
            var textBox = Request["comment_textBox"].ToString();
            var service = new AlbumFotoService();
            var picture = Request["nume_poza"].ToString();
            var madeBy = Request["madeBy"].ToString();
            if (textBox.Length != 0) {

                service.AdaugaComentariu(textBox, picture, madeBy);     
            }
            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult GetLink()
        {
            var service = new AlbumFotoService();
            var poza = Request["titlu_poza"].ToString();
            return View("Link", service.GetLink(poza));
        }
    }
}
