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
        static string _poza;

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
        public ActionResult IncarcaComentariu()
        {
            var service = new AlbumFotoService();
            string madeBy = Request["MadeBy"].ToString();
            string comentariu = Request["Comentariu"].ToString();
            if (null != madeBy && null != comentariu && null != _poza)
            {                
                service.IncarcaComentariu(comentariu, madeBy, _poza);
            }
            return View("Comentarii", service.VizualizareComentarii(_poza));
        }
        
        public ActionResult VizualizareComentarii(string poza)
        {
            _poza = poza;
            var service = new AlbumFotoService();
            return View("Comentarii", service.VizualizareComentarii(poza));
        }

        public ActionResult VizualizareLink(string poza)
        {
            _poza = poza;
            var service = new AlbumFotoService();
            return View("LinkPoza", service.VizualizareLink(poza));
        }
    }
}
