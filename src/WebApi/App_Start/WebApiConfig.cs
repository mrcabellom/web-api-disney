using Autofac.Integration.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(ContainerConfig.Build());
            ConfigureFormatters(config);
        }

        private static void ConfigureFormatters(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.JsonFormatter;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            jsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            jsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        }
    }
}
