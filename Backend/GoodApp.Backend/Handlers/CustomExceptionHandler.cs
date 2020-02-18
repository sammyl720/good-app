using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using GoodApp.Backend.Helpers;
using GoodApp.Data;
using GoodApp.Data.Models;

namespace GoodApp.Backend.Handlers
{
    class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var res = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occured, We'll fix it ASAP! Sorry, ^_^!")
            };
            context.Result = new ResponseMessageResult(res);
            LogHelper.Log(context.Request.RequestUri.AbsoluteUri, context.Exception.Message);
            //base.Handle(context); Check in test
        }
    }
}
