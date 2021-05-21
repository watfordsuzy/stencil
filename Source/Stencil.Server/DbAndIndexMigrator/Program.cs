using Codeable.Foundation.Core;
using SimpleMigrations;
using SimpleMigrations.Console;
using SimpleMigrations.DatabaseProvider;
using Stencil.Data.Sql;
using System;

namespace DbAndIndexMigrator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            CoreFoundation.Initialize(new DbAndIndexMigratorBootstrap(), true);

            var contextFactory = CoreFoundation.Current.SafeResolve<DbMigratorContextFactory>();

            using (var db = contextFactory.CreateDbConnection())
            {
                var databaseProvider = new MssqlDatabaseProvider(db);
                var migrator = new StencilSimpleMigrator(CoreFoundation.Current, databaseProvider);

                var consoleRunner = new ConsoleRunner(migrator)
                {
                    DefaultSubCommand = null,
                };

                return consoleRunner.Run(args);
            }
        }
    }
}
