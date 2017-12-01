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
        public ActionResult AddComments()
        {
                var service = new AlbumFotoService();
                string comment = Request["txtComment"].ToString();
                string picture = Request["picture"].ToString();
                //string user = Request["txtUserName"].ToString();
                
                    service.AddComment("remus", picture, comment);
                return View("Index", service.GetPoze());
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
        public ActionResult GetLink()
        {
                var service = new AlbumFotoService();
                var picture_name = Request["picture"].ToString();
                service.GetLink(picture_name);
                return View("Index", service.GetPoze());
        }

    }
}
