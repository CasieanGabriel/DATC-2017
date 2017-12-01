using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D1WebSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowConfig()
        {
            Trace.WriteLine("In ShowConfig action");
            return View();
        }

        public ActionResult GenerateError()
        {
            Trace.WriteLine("In GenerateError action");
			try
			{
				throw new NotImplementedException("This action is not implemented.");
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.ToString());
			}
			return View();
        }
    }
}
