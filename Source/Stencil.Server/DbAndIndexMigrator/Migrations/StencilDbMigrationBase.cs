using Codeable.Foundation.Common;
using Nest;
using SimpleMigrations;
using Stencil.Primary.Business.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator.DbMigrations
{
    public abstract class StencilDbMigrationBase : Migration
    {
        private readonly IFoundation _foundation;
        private readonly IStencilElasticClientFactory _clientFactory;

        protected string IndexName => _clientFactory.IndexName;

        protected StencilDbMigrationBase(IFoundation foundation)
        {
            _foundation = foundation ?? throw new ArgumentNullException(nameof(foundation));
            _clientFactory = _foundation.SafeResolve<IStencilElasticClientFactory>();
        }

        protected abstract void UpDatabase();

        protected abstract void UpIndex(ElasticClient client);

        protected override void Up()
        {
            this.UpDatabase();

            this.UpIndex(_clientFactory.CreateClient());
        }

        protected override void Down()
        {
            throw new NotSupportedException();
        }
    }
}
