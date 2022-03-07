using System.Web.Http;

namespace LeaveApp.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new System.Web.Mvc.HandleErrorAttribute());
        }
        public static void Configure(HttpConfiguration config)
        {
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
