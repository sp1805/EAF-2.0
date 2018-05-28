using System.Web.Mvc;
using System.Web.Routing;

namespace EAFProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
#if DEBUG
            {

                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
                );
            }
#else
            {

                routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Login", action = "Release", id = UrlParameter.Optional }
                );
            }
#endif

        }
    }
}