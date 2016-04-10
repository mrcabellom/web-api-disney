using CrossCutting.DocumentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace Core.Application.Attractions
{
    public class QueryAttractions : IQueryAttractions
    {
        public List<dynamic> FindAttractions()
        {
            var document = this.FindRecentAttraction();
            dynamic parameters = new ExpandoObject();
            parameters.date = document.createdAt;
            var sqlQueryAttractions = @"SELECT 
                c.attractionId,
                c.name,
                c.id,
                c.waitTime,
                c.type,
                c.createdAt                
                FROM c WHERE c.createdAt = @date";
            var documents = DocumentDBManagement.GetItemsAsync("attractionswaittime", sqlQueryAttractions, parameters);

            return documents;
        }

        public List<dynamic> FindAttractionsAggregate(string attractionId, DateTime startDate, DateTime endDate)
        {

            var sqlQuery = @"SELECT
                    c.attractionId,
                    c.name,
                    c.id,
                    c.waitTimeAvg,
                    c.date,
                    c.total
                    FROM c WHERE c.date >= @startDate
                    and c.date <= @endDate
                    and STARTSWITH(c.attractionId,@attractionId)";

            dynamic parameters = new ExpandoObject();
            parameters.attractionId = attractionId;
            parameters.startDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", startDate);
            parameters.endDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", endDate);

            var documents = DocumentDBManagement.GetItemsAsync("attractionswaittimeaggregation", sqlQuery, parameters);
            return documents;
        }

        public dynamic FindRecentAttraction()
        {
            var sqlQuery = @"SELECT TOP 1 * 
                             from c 
                             ORDER BY c.createdAt DESC";
            var documents = DocumentDBManagement.GetItemsAsync("attractionswaittime", sqlQuery);
            return documents.FirstOrDefault();
        }

    }
}
