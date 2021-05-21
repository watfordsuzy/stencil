using Codeable.Foundation.Common;
using Stencil.Common;
using Stencil.Common.Configuration;
using Stencil.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator
{
    public class DbMigratorContextFactory : StencilContextFactory
    {
        public DbMigratorContextFactory(IFoundation foundation)
            : base(foundation)
        {
        }

        public DbConnection CreateDbConnection()
        {
            return base.ExecuteFunction(nameof(CreateDbConnection), delegate ()
            {
                string connectionString = this.GetSqlConnectionString();
                return new SqlConnection(connectionString);
            });
        }

        protected override string GetSqlConnectionString()
        {
            return base.ExecuteFunction<string>(nameof(GetSqlConnectionString), delegate ()
            {
                return this.ConnectionStringCache.PerFoundation("DbMigratorContextFactory.GetSqlConnectionString", delegate ()
                {
                    ISettingsResolver settingsResolver = this.IFoundation.Resolve<ISettingsResolver>();
                    string connectionString = settingsResolver.GetSetting(CommonAssumptions.APP_KEY_SQL_DB);
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new Exception("Connection string not found: " + CommonAssumptions.APP_KEY_SQL_DB);
                    }
                    this.IFoundation.LogWarning("ConnectionString: " + connectionString);
                    return connectionString;
                });
            });
        }
    }
}
