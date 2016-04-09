using Core.Application.Attractions;
using CrossCutting.RedisCache;
using System;
using System.Web.Http;
using WebApi.Infrastructure.Constraints;

namespace WebApi.Controllers
{
    [VersionedRoutePrefix("api/attractions", 2)]
    public class AttractionsV2Controller : ApiController
    {
        private readonly ICache _cache;
        private readonly IQueryAttractions _queryAttractions;
        public AttractionsV2Controller(ICache cache, IQueryAttractions queryAttractions)
        {
            _cache = cache;
            _queryAttractions = queryAttractions;
        }

        [VersionedRoute("", 2)]
        [Authorize]
        public IHttpActionResult GetAttractions([FromUri] Boolean cache)
        {
            dynamic attraction = new
            {
                values = new[] {
                    "attraction1",
                    "attraction2"
                }
            };

            return Ok(attraction);
        }
        [VersionedRoute("aggregate", 2)]
        public IHttpActionResult GetAttractionsAggregate(
            [FromUri] DateTime startDate,
            [FromUri] DateTime endDate)
        {
            dynamic attractions = new
            {
                values = new[] {
                    "attraction1",
                    "attraction2"
                }
            };
            return Ok(attractions);
        }

    }
}
