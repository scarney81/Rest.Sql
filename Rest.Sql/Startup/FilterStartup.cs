using System.Web.Mvc;
using Rest.Sql.Startup;

[assembly: WebActivator.PostApplicationStartMethod(typeof(FilterStartup), "RegisterFilters")]
namespace Rest.Sql.Startup
{
    public class FilterStartup
    {
        public static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }
}