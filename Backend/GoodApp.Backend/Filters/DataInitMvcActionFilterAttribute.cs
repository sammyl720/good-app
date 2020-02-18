using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GoodApp.Backend.Controllers;
using GoodApp.Backend.Helpers;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace GoodApp.Backend.Filters
{
    public class DataInitMvcActionFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controller = (BaseMvcController)filterContext.Controller;
            ControllerHelper.InitCurrentAccess(controller);
            controller.ViewBag.CurrentAccess = controller.CurrentAccess;
        }
    }
}
