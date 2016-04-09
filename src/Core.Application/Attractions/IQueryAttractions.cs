using System;
using System.Collections.Generic;

namespace Core.Application.Attractions
{
    public interface IQueryAttractions
    {
        List<dynamic> FindAttractions();
        List<dynamic> FindAttractionsAggregate(string attractions, DateTime startDate, DateTime endDate);
    }
}
