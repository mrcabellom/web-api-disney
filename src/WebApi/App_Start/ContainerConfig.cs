using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Infrastructure.ContainerModules;

namespace WebApi
{
    public class ContainerConfig
    {
        static IContainer _container;
        public static ILifetimeScope Build()
        {
            var containerBuilder = new ContainerBuilder();

            //Add modules

            containerBuilder.RegisterModule<CacheModule>();
            containerBuilder.RegisterModule<HttpApiModule>();
            containerBuilder.RegisterModule<MiscModule>();

            //build container
            _container = containerBuilder.Build();

            return _container;
        }
    }
}