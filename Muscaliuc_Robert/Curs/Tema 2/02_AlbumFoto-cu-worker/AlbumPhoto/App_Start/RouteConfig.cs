using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlbumPhoto
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Page",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Comentarii", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AnotherPage",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "LinkAcces", id = UrlParameter.Optional }
            );
        }
    }
}