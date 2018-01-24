using AlbumPhoto.Models;
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
            if (file != null && file.ContentLength > 0)
            {
                service.IncarcaPoza("guest", file.FileName, file.InputStream);
            }

            return View("Index", service.GetPoze());
        }

        [HttpPost]
        public ActionResult AddComment(string fileName, string comment)
        {
            var fotoService = new AlbumFotoService();
            fotoService.AddComment(fileName, comment);
            return View("Index", fotoService.GetPoze());
        }

        //varianta de generare facuta de mine 

        //[HttpGet]
        //public ActionResult Picture(long key, string fileName)
        //{
        //    var date = new DateTime(key);
        //    var dateDiff = DateTime.UtcNow.Subtract(date).TotalHours;

        //    if (dateDiff > 2.0f)
        //    {
        //        return View("PozaIndiv", new Poza
        //        {
        //            Url = string.Empty
        //        });
        //    }

        //    var fotoService = new AlbumFotoService();
        //    var poze = fotoService.GetPoze();
        //    //var poza = poze.FirstOrDefault(p => p.Poza.Url.Contains(fileName));
        //    var poza = poze.FirstOrDefault(p => p.Poza.Description == fileName);

        //    if (poza == null)
        //    {
        //        return View("PozaIndiv", new Poza
        //        {
        //            Url = string.Empty
        //        });
        //    }
        //    return View("PozaIndiv", poza.Poza);
        //}

        [HttpGet]
        public ActionResult Picture(long key, string fileName)
        {
            var fotoService = new AlbumFotoService();
            var poze = fotoService.GetPoze();
            var poza = poze.FirstOrDefault(p => p.Poza.Description == fileName);
            var url = poza.Poza.Url;
            var secureUrl = BlobHandler.GetBlobSasUri(url, fileName);
            var encodedUrl = BlobHandler.Base64Encode(secureUrl);
            return View("PozaIndiv", new Poza()
            {
                Url = secureUrl
            });
        }
    }
}
