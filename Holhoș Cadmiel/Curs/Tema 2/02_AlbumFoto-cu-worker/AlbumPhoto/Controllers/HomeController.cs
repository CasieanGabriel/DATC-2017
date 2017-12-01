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

        public ActionResult ViewComments(string picture_name)
        {
            var foto_service = new AlbumFotoService();
            return View(foto_service.GetComments(picture_name));
        }

        public ActionResult AddComments()
        {
            var service = new AlbumFotoService();
            string User = Request["User"].ToString();
            string Comentariu = Request["Comment"].ToString();
            string Nume_poza = Request["picture_name"].ToString();

            if(!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(Comentariu))
            {
                service.AdaugaComentariu(User, Comentariu, Nume_poza);
            }

            return View("Index", service.GetPoze());
        }
        

}
}
