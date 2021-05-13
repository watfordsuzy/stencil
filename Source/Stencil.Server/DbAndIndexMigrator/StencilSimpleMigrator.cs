using Codeable.Foundation.Common;
using DbAndIndexMigrator.DbMigrations;
using Microsoft.Practices.Unity;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator
{
    public class StencilSimpleMigrator : SimpleMigrator
    {
        private readonly IFoundation _foundation;

        public StencilSimpleMigrator(IFoundation foundation, IDatabaseProvider<DbConnection> databaseProvider)
            : base(new UnityMigrationProvider(foundation), databaseProvider)
        {
            _foundation = foundation ?? throw new ArgumentNullException(nameof(foundation));
        }

        protected override IMigration<DbConnection> CreateMigration(MigrationData migrationData)
            => _foundation.Container.Resolve<StencilDbMigrationBase>(migrationData.GetMigrationName());
    }
}
