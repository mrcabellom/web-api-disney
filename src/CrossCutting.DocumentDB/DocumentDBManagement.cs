using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Dynamic;


namespace CrossCutting.DocumentDB
{
    public static class DocumentDBManagement
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["DocumentDBDatabase"];
        private static DocumentClient client;

        static DocumentDBManagement()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["DocumentDBHost"]), ConfigurationManager.AppSettings["DocumentDBAuthKey"]);
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync(string CollectionId)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        public static List<dynamic> GetItemsAsync(string CollectionId, string query, ExpandoObject parameters = null)
        {
            var sqlParameters = new SqlParameterCollection();
            var listResult = new List<dynamic>();
            SqlParameter sqlParam;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    sqlParam = new SqlParameter("@" + kvp.Key, kvp.Value);
                    sqlParameters.Add(sqlParam);
                }
            }

            SqlQuerySpec sqlQueryDocumentDB = new SqlQuerySpec
            {
                QueryText = query,
                Parameters = sqlParameters
            };

            listResult = client.CreateDocumentQuery(
               UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
               sqlQueryDocumentDB).AsEnumerable().ToList();

            return listResult;
        }
    }
}
