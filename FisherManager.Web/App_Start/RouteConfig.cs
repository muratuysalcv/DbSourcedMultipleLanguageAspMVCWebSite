using System.Web.Mvc;
using System.Web.Routing;

namespace FisherManager.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Eski FisherManager web sitesi için var olan SEO'yu kaybetmemek adına bu işlemi yapıyoruz.
            routes.MapRoute(
               name: "SeoFriendlyRedirect1",
               url: "sayfa/icerigi/{SeoUrl}",
               defaults: new { controller = "Home", action = "Redirect", SeoUrl = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "SeoFriendlyUrl",
               url: "{culture}/{SeoUrl}",
               defaults: new { controller = "Home", action = "Index", SeoUrl = UrlParameter.Optional, culture = "en-US" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, culture = "en-US" }
            );
            routes.MapRoute(
                name: "Management",
                url: "{culture}/Management/{action}/{id}",
                defaults: new { controller = "Management", action = "Login", id = UrlParameter.Optional, culture = "en-US" }
            );

        }
    }
}
