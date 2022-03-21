using ParkingSolution.WebAPI.Models.Security;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace ParkingSolution.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Filters.Add(new BasicAuthenticationAttribute());
        }
    }
}
