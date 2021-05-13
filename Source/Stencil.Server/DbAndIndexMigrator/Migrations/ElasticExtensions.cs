using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator.Migrations
{
    public static class ElasticExtensions
    {
        public static TResponse ThrowIfUnsuccessful<TResponse>(this TResponse response)
            where TResponse : IResponse
        {
            if (!response.IsValid)
            {
                throw new InvalidOperationException($"Problem with ElasticSearch update: {response}");
            }

            return response;
        }
    }
}
