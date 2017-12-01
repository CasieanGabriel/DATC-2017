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
        public ActionResult ViewComm(string pictureName)
        {
            var service = new AlbumFotoService();
            return View(service.GetComentariu(pictureName));
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
        public ActionResult IncarcaComentarii()
        {
            var service = new AlbumFotoService();
            string user = Request["usertxt"].ToString();
            string comentariu = Request["commtxt"].ToString();
            if(user!=null && comentariu!=null)
            {
                service.IncarcaComentariu(user,comentariu);

            }
            return
                 View("Index", service.GetPoze());
        }
        
        public ActionResult GenerateLink(string pictureName)
        {
            var service = new AlbumFotoService();
            string link = service.GenerateLink(pictureName);
            ViewData["LINK"] = link;
            return View();
        }
    }
}
