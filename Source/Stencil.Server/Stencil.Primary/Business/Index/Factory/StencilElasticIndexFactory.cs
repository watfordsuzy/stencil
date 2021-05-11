using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stencil.Primary.Business.Index
{
    public partial class StencilElasticIndexFactory
    {
        public void BeforeIndexCreation(CreateIndexDescriptor indexer)
        {
            this.MapIndexModels(indexer);
        }

        partial void MapIndexModels(CreateIndexDescriptor indexer);
    }
}
