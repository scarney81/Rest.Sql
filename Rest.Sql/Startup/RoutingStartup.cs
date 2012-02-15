using System.Web.Mvc;
using System.Web.Routing;
using Rest.Sql.Startup;

[assembly: WebActivator.PostApplicationStartMethod(typeof(RoutingStartup), "RegisterRoutes")]
namespace Rest.Sql.Startup
{
    public class RoutingStartup
    {
        private static RouteCollection _routes;

        static RoutingStartup()
        {
            _routes = RouteTable.Routes;
        }

        public RoutingStartup(RouteCollection routes)
        {
            _routes = routes;
        }

        public static void RegisterRoutes()
        {
            _routes.Clear();
            _routes.RouteExistingFiles = true;

            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            _routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });

            _routes.MapRoute("table", "{table}", new { controller = "query", action = "table", table = UrlParameter.Optional });
        }
    }
}
