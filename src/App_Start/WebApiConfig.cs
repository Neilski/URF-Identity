using System.Web.Http;
using Newtonsoft.Json.Serialization;


namespace UrfIdentity.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services


            #region Configure Formatters
            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Configure JSON output to how we want it
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            // camelCase JSON properties
            json.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Use custom date converter to convert UTC dates to local dates
            // see documentation for the LocalDateTimeConverter() class
            // config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
            //    new UtcToLocalDateTimeConverter());

#if DEBUG
            // Provide nicely formatted data to help when debugging
            json.SerializerSettings.Formatting =
                Newtonsoft.Json.Formatting.Indented;
#endif
            #endregion Configure Formatters


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "Api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional});

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );
        }
    }
}