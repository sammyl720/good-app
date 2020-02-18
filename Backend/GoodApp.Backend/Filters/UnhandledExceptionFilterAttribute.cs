using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using GoodApp.Backend.Helpers;
using GoodApp.Data;

namespace GoodApp.Backend.Filters
{
    public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotImplemented);
            }
            else
            {
#if DEBUG
                base.OnException(actionExecutedContext);
#else
                var res = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occured, We'll fix it ASAP! Sorry, ^_^!")
                };
                actionExecutedContext.Response = res;          
#endif
            }
            LogHelper.Log(actionExecutedContext.Request.RequestUri.AbsoluteUri, actionExecutedContext.Exception.Message);
            //base.OnException(actionExecutedContext);
        }
    }
}
