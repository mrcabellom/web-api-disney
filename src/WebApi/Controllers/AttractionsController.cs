using Core.Application.Attractions;
using CrossCutting.RedisCache;
using System;
using System.Web.Http;
using WebApi.Infrastructure.Constraints;

namespace WebApi.Controllers
{
    [VersionedRoutePrefix("api/attractions", 1)]
    public class AttractionsController : ApiController
    {
        private readonly ICache _cache;
        private readonly IQueryAttractions _queryAttractions;
        public AttractionsController(ICache cache, IQueryAttractions queryAttractions)
        {
            _cache = cache;
            _queryAttractions = queryAttractions;
        }

        [VersionedRoute("", 1)]
        public IHttpActionResult GetAttractions()
        {
            dynamic attraction = _cache.Get<dynamic>($"attractions");
            if (attraction == null)
            {
                attraction = _queryAttractions.FindAttractions();
                _cache.Insert($"attractions", attraction);
            }


            return Ok(attraction);
        }
        [VersionedRoute("aggregate", 1)]
        public IHttpActionResult GetAttractionsAggregate(
            [FromUri] string attractionId,
            [FromUri] DateTime startDate,
            [FromUri] DateTime endDate)
        {
            dynamic attractions = _queryAttractions.FindAttractionsAggregate(attractionId, startDate, endDate);
            return Ok(attractions);
        }
    }
}
