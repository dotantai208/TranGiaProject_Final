using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SupTranGiaTichDiem
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
    name: "FilterByCategory",
    url: "{controller}/{action}/{maloai}/{page}",
    defaults: new { controller = "Home", action = "ListSanPham", maloai = UrlParameter.Optional, page = UrlParameter.Optional }
);


        }
    }
}
