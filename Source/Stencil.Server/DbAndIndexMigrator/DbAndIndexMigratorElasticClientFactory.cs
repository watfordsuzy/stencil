using Codeable.Foundation.Common;
using Nest;
using Stencil.Primary.Business.Index;

namespace DbAndIndexMigrator
{
    public class DbAndIndexMigratorElasticClientFactory : StencilElasticClientFactory
    {
        public DbAndIndexMigratorElasticClientFactory(IFoundation foundation)
            : base(foundation)
        {
        }

        public override ElasticClient CreateClient()
        {
            return base.ExecuteFunction(nameof(CreateClient), delegate ()
            {
                ElasticClient result = new ElasticClient(base.ConnectionSettings);
                return result;
            });
        }
    }
}