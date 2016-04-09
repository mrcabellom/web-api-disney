using Autofac;
using Core.Application.Attractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace WebApi.Infrastructure.ContainerModules
{
    public class MiscModule
       : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var assemblies = new[]
           {
                typeof(ContainerConfig).Assembly, //this
                typeof(IQueryAttractions).Assembly
            };

            builder.RegisterAssemblyTypes(assemblies)
                .Where(x => x.BaseType != typeof(ExceptionHandler) && x.BaseType != typeof(ExceptionLogger))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}