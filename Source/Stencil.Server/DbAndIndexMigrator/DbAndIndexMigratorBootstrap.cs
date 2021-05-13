using Codeable.Foundation.Common;
using Codeable.Foundation.Common.System;
using Codeable.Foundation.Core;
using Codeable.Foundation.Core.System;
using DbAndIndexMigrator.DbMigrations;
using Microsoft.Practices.Unity;
using SimpleMigrations;
using Stencil.Common.Configuration;
using Stencil.Data.Sql;
using Stencil.Primary.Business.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator
{
    public class DbAndIndexMigratorBootstrap : CoreBootStrap
    {
        public override void OnAfterSelfRegisters(IFoundation foundation)
        {
            base.OnAfterSelfRegisters(foundation);

            foundation.Container.RegisterType<ISettingsResolver, AppConfigSettingsResolver>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IStencilContextFactory, DbMigratorContextFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<DbMigratorContextFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IStencilElasticClientFactory, DbAndIndexMigratorElasticClientFactory>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<DbAndIndexMigratorElasticClientFactory>(new ContainerControlledLifetimeManager());

            // Replace Exception Handlers
            foundation.Container.RegisterType<IHandleException, StandardThrowExceptionHandler>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterType<IHandleExceptionProvider, StandardThrowExceptionHandlerProvider>(new ContainerControlledLifetimeManager());
            foundation.Container.RegisterInstance<StandardThrowExceptionHandlerProvider>(new StandardThrowExceptionHandlerProvider(foundation.GetLogger()), new ContainerControlledLifetimeManager());

            this.RegisterMigrations(foundation);
        }

        private void RegisterMigrations(IFoundation foundation)
        {
            var migrationTypes = typeof(Program).Assembly.GetTypes()
                                                         .Where(IsMigration)
                                                         .ToArray();

            foreach (var migrationType in migrationTypes)
            {
                MigrationAttribute attribute = migrationType.GetMigrationAttribute();

                foundation.Container.RegisterType(typeof(StencilDbMigrationBase), migrationType, attribute.GetMigrationName());
            }

            bool IsMigration(Type type)
            {
                return type.GetCustomAttributes(typeof(MigrationAttribute), inherit: false).Any();
            }
        }
    }
}
