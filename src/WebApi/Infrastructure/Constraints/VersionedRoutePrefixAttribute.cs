using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Properties.Resources;

namespace WebApi.Infrastructure.Constraints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal sealed class VersionedRoutePrefixAttribute : RoutePrefixAttribute
    {
        public VersionedRoutePrefixAttribute(string template, int version)
            : base(template)
        {
            if (version < 1)
            {
                throw Error.ArgumentOutOfRange("version");
            }

            Version = version;
        }

        public int Version { get; private set; }
    }
}
