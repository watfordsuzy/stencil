using Codeable.Foundation.Common;
using Nest;
using Stencil.Primary.Business.Index;

namespace ElasticMappingGenerator
{
    public class MappingGeneratorElasticClientFactory : StencilElasticClientFactory
    {
        public MappingGeneratorElasticClientFactory(IFoundation foundation) 
            : base(foundation)
        {
        }

        public ConnectionSettings BuildConnectionSettings()
            => base.ConnectionSettings;

        public ICreateIndexRequest BuildCreateIndexRequest()
            => base.BuildCreateIndexRequest(new CreateIndexDescriptor(base.IndexName));
    }
}
