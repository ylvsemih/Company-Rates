using CompanyRatesAPI.Models;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CompanyRatesAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            using (var db = new CompanyRatesAPIContext())
            {
                db.Database.CreateIfNotExists();
            }
            // Web API configuration and services
            //config.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "ControllerAndAction",
            //    routeTemplate: "api/{controller}/{action}"
            //);

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}