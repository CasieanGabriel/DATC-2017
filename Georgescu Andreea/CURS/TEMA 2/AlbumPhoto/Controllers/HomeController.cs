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
        public ActionResult AddComment()

        {
            var service = new AlbumFotoService();
            var userName = Request["UserName"].ToString();
            var text = Request["Comment"].ToString();
            var description = Request["picName"].ToString();
            if(!string.IsNullOrEmpty(userName)&& !string.IsNullOrEmpty(text))
            {
                service.AddComment(userName, text, description);
            }
            return View("Index", service.GetPoze());
        }


        [HttpPost]
        public ActionResult ViewComments()
        {

            var service = new AlbumFotoService();
            var poza = Request["picName"].ToString();
            return View(service.GetComment(poza));
        }


        [HttpPost]
        public ActionResult GetLink()
        {

            var service = new AlbumFotoService();
            var poza = Request["picName"].ToString();
            string link = service.GetLink(poza);
            ViewData["LINK"] = link;
            return View();
        }
    }
}
