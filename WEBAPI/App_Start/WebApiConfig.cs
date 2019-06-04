using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WEBAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });


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



            //USE CORS- Cross Origin Resource Sharing OR JSONP (Json padding) to allow APIs to work in cross origins.
            //   meaning not on the same port...


            //using JSONP
            //use type: jsonp in html pages
            //isntall package webApiContrib.formatting.jsonp
            //when using this specify accept:application/json in fiddler..
            //or provide http://localhost:60650/api/Employee?callback=abc ..json will be wrapped inside abc object.
            //config.Formatters.Insert(0, new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter));

            //using CORS
            //use type: json in html pages
            //instal package microsft....cors
            //provide those websites whom u want to consume your APIs..
            //("www.locahost.com,www.google.com,www.india.com","headers allow or not","GET, or PUT or POST")
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);



            //however here enabling CORS in this webAPI is global to all controllers.

            //for enabling CORS for individual controllers use

            //[EnableCorsAttribute("*","*","*")] at the class level
        }
    }



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

}
