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

        public ActionResult ViewComments(string pic_name)
        {
            var service = new AlbumFotoService();
            return View(service.GetComments(pic_name));
        }

        [HttpPost]
        public ActionResult AddComments()
        {
            var service = new AlbumFotoService();
            string User = Request["User"].ToString();
            string Comment = Request["Comment"].ToString();
            string pic_name = Request["picName"].ToString();
            if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Comment))
            {
                service.AddComment(User, Comment, pic_name);
            }
            return View("Index", service.GetPoze());
        }

        public ActionResult GetLink(string pic_name)
        {
            var service = new AlbumFotoService();
            string link = service.GetLink(pic_name);
            ViewData["LINK"] = link;
            return View();
        }
    }
}
