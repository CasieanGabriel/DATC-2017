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
        public ActionResult PostComment(string name, string comment, string user)
        {
            var service = new AlbumFotoService();
            if (comment != null && user != null)
            {
                service.ComentOnPicture(name, Guid.NewGuid().ToString(), comment, user);
            }
            return RedirectToAction("Index");

        }
        public ActionResult ReadComments(string pictureName)
       {
            try
            {
                var service = new AlbumFotoService();
                return View(service.GetComments(pictureName));
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

    }
}
