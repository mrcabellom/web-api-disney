using System.Linq;
using System.Web.Http;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using WebActivatorEx;
using WebApi;
using WebApi.Infrastructure.Constraints;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.Schemes(new[] { "http" });
                       
                        c.MultipleApiVersions(
                           (apiDesc, targetApiVersion) =>
                           {

                               var apiVersion = apiDesc.GetControllerAndActionAttributes<VersionedRoutePrefixAttribute>()
                                     .SingleOrDefault();

                               if (apiVersion != null)
                               {
                                   return String.Equals(targetApiVersion, string.Format("v{0}", apiVersion.Version),
                                  StringComparison.InvariantCultureIgnoreCase);
                               }
                               else
                               {
                                   return false;
                               }
                           },
                           (vc) =>
                           {
                               vc.Version("v1", "DisneyApiAzureBootCamp API v1")
                                   .Description("Disey HTTP API");

                               vc.Version("v2", "DisneyApiAzureBootCamp API v2")
                                 .Description("Disey HTTP API");
                           });

                        c.GroupActionsBy(apiDesc => apiDesc.ActionDescriptor.ControllerDescriptor.ControllerName);
                        c.DescribeAllEnumsAsStrings();

                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    })
                    .EnableSwaggerUi(c =>
                      {

                          c.InjectJavaScript(thisAssembly, "WebApi.Infrastructure.Swagger.SwaggerRequest.js");

                          c.BooleanValues(new[] { "0", "1" });

                          c.DocExpansion(DocExpansion.List);
                          c.EnableDiscoveryUrlSelector();
                      });
        }
    }   
}