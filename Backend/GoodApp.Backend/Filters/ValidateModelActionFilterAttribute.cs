using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GoodApp.Backend.Filters
{
    public class ValidateModelActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
    }
}
