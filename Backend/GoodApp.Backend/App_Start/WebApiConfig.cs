using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Tracing;
using GoodApp.Backend.Filters;
using GoodApp.Backend.Handlers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using TechwuliArsenal.WebApi.MultipartDataMediaFormatter;
using ITraceWriter = System.Web.Http.Tracing.ITraceWriter;

namespace GoodApp.Backend
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            };

            var existedJsonFormatters =
                config.Formatters.Where(
                    p => p.SupportedMediaTypes.Any(m => m.MediaType == "application/json")).ToList();
            foreach (var existedJsonFormatter in existedJsonFormatters)
            {
                config.Formatters.Remove(existedJsonFormatter);
            }

            config.Formatters.Add(jsonFormatter);

            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());

            var traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Debug;

            config.EnableCors();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Filters.Add(new UnhandledExceptionFilterAttribute());
            config.Filters.Add(new ValidateModelActionFilterAttribute());
            config.Filters.Add(new DataInitActionFilterAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
#if DEBUG
            config.Services.Replace(typeof (ITraceWriter), new TraceWriter());
#endif
            config.Services.Replace(typeof (IExceptionHandler), new CustomExceptionHandler());

            config.MessageHandlers.Add(new AppTokenMessageHandler());
        }
    }


    public class TraceWriter : ITraceWriter
    {
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            var rec = new TraceRecord(request, category, level);
            traceAction(rec);
            WriteTrace(rec);
        }

        protected void WriteTrace(TraceRecord rec)
        {
            var message = string.Format("Operator: {0}; Operation: {1}; Message: {2}",
                rec.Operator, rec.Operation, rec.Message);
            System.Diagnostics.Trace.WriteLine(message, rec.Category);
        }
    }
}