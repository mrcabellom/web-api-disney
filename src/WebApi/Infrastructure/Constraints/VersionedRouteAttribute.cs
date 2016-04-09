using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using WebApi.Properties.Resources;

namespace WebApi.Infrastructure.Constraints
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class VersionedRouteAttribute : Attribute, IDirectRouteFactory, IHttpRouteInfoProvider
    {

        private const int NO_VERSION = 0;
        public VersionedRouteAttribute(string template)
        {
            if (template == null)
            {
                throw Error.ArgumentNull("template");
            }

            Template = template;
            Version = NO_VERSION;
        }

        public VersionedRouteAttribute(string template, int version)
        {
            if (template == null)
            {
                throw Error.ArgumentNull("template");
            }

            if (version <= NO_VERSION)
            {
                throw Error.ArgumentOutOfRange("version");
            }

            Template = template;
            Version = version;
        }

        public string Name { get; set; }

        public int Order { get; set; }

        public string Template { get; private set; }

        public int Version { get; private set; }

        public RouteEntry CreateRoute(DirectRouteFactoryContext context)
        {
            if (context == null)
            {
                throw Error.ArgumentNull("context");
            }

            // Current action method
            var actionDescriptor = context.Actions.First();

            var currentVersion = GetVersion(actionDescriptor);

            IDirectRouteBuilder builder = context.CreateBuilder(Template);

            builder.Constraints.Add(
                new KeyValuePair<string, object>("version", new ApiVersionConstraint(currentVersion)));
            builder.Name = Name;
            builder.Order = Order;

            return builder.Build();
        }

        private static int GetVersion(HttpActionDescriptor actionDescriptor)
        {
            int version = 1;

            var controllerAttribute = actionDescriptor.ControllerDescriptor
             .GetCustomAttributes<VersionedRoutePrefixAttribute>(false)
             .FirstOrDefault();

            if (controllerAttribute != null)
            {
                version = controllerAttribute.Version;
            }

            var actionAttribute = actionDescriptor.GetCustomAttributes<VersionedRouteAttribute>(false)
                .FirstOrDefault();

            if (actionAttribute != null && actionAttribute.Version != NO_VERSION)
            {
                version = actionAttribute.Version;
            }

            return version;
        }
    }
}