using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace LeaveApp.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(true);
           // config.MessageHandlers.Add(new PreflightRequestsHandler());

        }

        public class PreflightRequestsHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request.Headers.Contains("Origin") && request.Method.Method == "OPTIONS")
                {
                    var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Authorization");
                    response.Headers.Add("Access-Control-Allow-Methods", "*");
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
