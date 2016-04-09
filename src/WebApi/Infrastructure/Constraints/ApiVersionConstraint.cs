using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace WebApi.Infrastructure.Constraints
{
    public class ApiVersionConstraint : IHttpRouteConstraint
    {

        public const string API_VERSION_HEADER_NAME = "X-Api-Version";

        private const int DefaultVersion = 1;


        public ApiVersionConstraint(int managedVersion)
        {
            ManagedVersion = managedVersion;
        }

        public int ManagedVersion { get; private set; }

        public bool Match(
            HttpRequestMessage request,
            IHttpRoute route, string parameterName,
            IDictionary<string, object> values,
            HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                int version = GetVersionFromHeader(request) ?? DefaultVersion;
                return version == ManagedVersion;
            }

            return false;
        }

        private int? GetVersionFromHeader(HttpRequestMessage request)
        {
            string versionAsString;
            IEnumerable<string> headerValues;

            if (request.Headers.TryGetValues(API_VERSION_HEADER_NAME, out headerValues)
                &&
                headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
            }
            else
            {
                return null;
            }

            int version;

            if (versionAsString != null && int.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }
    }
}