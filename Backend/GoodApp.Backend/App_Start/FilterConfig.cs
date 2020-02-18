using System.Web;
using System.Web.Mvc;
using GoodApp.Backend.Filters;

namespace GoodApp.Backend
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorMvcFilterAttribute());
            filters.Add(new DataInitMvcActionFilterAttribute());
        }
    }
}
