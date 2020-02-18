using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using GoodApp.Backend.Api.Controllers;
using GoodApp.Backend.Controllers;
using GoodApp.Backend.Helpers;
using GoodApp.Data;
using GoodApp.Data.Repositories;
using Microsoft.AspNet.Identity;

namespace GoodApp.Backend.Filters
{
    public class DataInitActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
            IEnumerable<string> appTokens;
            actionContext.Request.Headers.TryGetValues("appToken", out appTokens);
            string appToken = appTokens.FirstOrDefault();
            var controller = (BaseApiController)actionContext.ControllerContext.Controller;
            var application = controller.RepositoryProvider.Get<ApplicationRepository>().FirstOrDefault(p=>p.Token.ToUpper() == appToken.ToUpper());
            if (application == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest,"app token not found");
                return;
            }
            if (application.Status == Enums.ApplicationStatus.Inactive.ToString())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "app token is not active");
                return;
            }
            controller.CurrentAccess = new CurrentAccess()
            {
                ApplicationId = application.ApplicationId,
                ApplicationType = application.Type
            };
            ControllerHelper.InitCurrentAccess(controller);
        }
    }
}
