using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Integration.WebApi;

namespace WebApi.Infrastructure.ContainerModules
{
    public class HttpApiModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new[]
            {
                typeof(ContainerConfig).Assembly
            };

            builder.RegisterApiControllers(assemblies)
                .InstancePerRequest();
        }
    }
}