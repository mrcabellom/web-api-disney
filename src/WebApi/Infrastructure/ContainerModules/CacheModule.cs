using Autofac;
using CrossCutting.RedisCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Infrastructure.ContainerModules
{
    public class CacheModule
       : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            bool enabledRedisCache = true;

            if (!enabledRedisCache)
            {
                builder.RegisterType<NoCache>()
                    .AsImplementedInterfaces()
                    .InstancePerRequest();
            }
            else
            {
                builder.RegisterType<ServiceStackRedisCache>()
                   .AsImplementedInterfaces()
                   .InstancePerRequest();
            }
        }
    }
}