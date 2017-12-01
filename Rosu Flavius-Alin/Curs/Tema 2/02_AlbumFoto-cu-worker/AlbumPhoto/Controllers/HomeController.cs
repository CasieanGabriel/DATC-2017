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
        public ActionResult AddComment(string comm, string photo)
        {
            var service = new AlbumFotoService();
            if(comm != null && comm.Length>0)
            {
                service.AddComment("guest", photo, comm);
            }
            return View("Index", service.GetPoze());

        }
        [HttpPost]
        public ActionResult GetComments(string photo)
        {
            var service = new AlbumFotoService();
            
            return View("GetComments", service.GetComments(photo)); 
        }
        [HttpPost]
        public ActionResult GetLink(string photo)
        {
            var service = new AlbumFotoService();
            return View("Link", service.GetLink(photo));
        }
        
    }
}
