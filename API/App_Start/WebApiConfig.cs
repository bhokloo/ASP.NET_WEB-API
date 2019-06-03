using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace API
{
    public static class WebApiConfig
    {
        public class CustonJsonFormatter : JsonMediaTypeFormatter
        {
            public CustonJsonFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            }
            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

        }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Employee", id = RouteParameter.Optional }
            );

            //RETURN ONLY JSON.
            //config.Formatters.Remove(config.Formatters.XmlFormatter);

            //RETURN ONLY XML.
            //config.Formatters.Remove(config.Formatters.JsonFormatter);


            //return JSON when from web browser. but in fiddler the content type is still text/html and not json.
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            //to overcome the problem of content type..
            //config.Formatters.Add(new CustonJsonFormatter());


            //for providing JSON view to web service and camel case...default is pascal case.
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}
