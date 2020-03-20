using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Store
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("ProductDetails", "Home/{action}/{id}", new { controller = "Home", action = "ProductDatalies", id = UrlParameter.Optional },
                          new[] { "Store.Controllers" });

            routes.MapRoute("Categories", "Shop/{action}/{id}", new { controller = "Shop", action = "Categories", id = UrlParameter.Optional },
                           new[] { "Store.Controllers" });

            routes.MapRoute("Products", "Shop/{action}/{id}", new { controller = "Shop", action = "Products", id = UrlParameter.Optional },
                            new[] { "Store.Controllers" });

            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                            new[] { "Store.Controllers" });

            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                             new[] { "Store.Controllers" });

            routes.MapRoute("CategoryMenuPartial", "Dashboard/{action}/{name}", new { controller = "Dashboard", action = "Index", name = UrlParameter.Optional },
                           new[] { "Store.Controllers" });

            routes.MapRoute("Default", "", new { controller = "Home", action = "Index" }, new[] { "Store.Controllers" });
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            //);
        }
    }
}
