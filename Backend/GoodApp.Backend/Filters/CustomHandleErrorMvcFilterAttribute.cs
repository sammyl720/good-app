using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodApp.Backend.Helpers;
using GoodApp.Data;

namespace GoodApp.Backend.Filters
{
    public class CustomHandleErrorMvcFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            var reqest = filterContext.HttpContext.Request.Url == null
                ? filterContext.HttpContext.Request.Path
                : filterContext.HttpContext.Request.Url.AbsoluteUri;
            LogHelper.Log(reqest, filterContext.Exception.Message);
        }
    }
}
