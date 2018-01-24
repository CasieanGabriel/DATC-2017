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
            return View(service.Get_Picture());
        }

        [HttpPost]
        public ActionResult IncarcaPoza(HttpPostedFileBase file)
        {
            var service = new AlbumFotoService();
            if (file!=null && file.ContentLength > 0)
            {
                service.Load_Picture("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.Get_Picture());
        }

        [HttpPost]
        public ActionResult AddComment(string user, string comment, string img)
        {
            var service = new AlbumFotoService();
            if (comment != "" && img != "" && user != "")
            {
                service.AddComment(user, comment, img);
            }
            return View("Index", service.Get_Picture());
        }
        [HttpPost]
        public ActionResult GenerateLink(string img)
        {
            var service = new AlbumFotoService();
            return View("GenerateLink", service.GenerateLink(img));
        }
        [HttpPost]
        public ActionResult GenLike(string img)
        {
            var service = new AlbumFotoService();
            return View("Like", service.ViewLike(img));
        }

        [HttpGet]
        public ActionResult ViewComments(string img)
        {
            var service = new AlbumFotoService();
            return View("ViewComments", service.ViewComments(img));
        }
    }
}
