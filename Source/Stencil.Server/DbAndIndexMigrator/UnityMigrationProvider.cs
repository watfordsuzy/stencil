using Codeable.Foundation.Common;
using DbAndIndexMigrator.DbMigrations;
using Microsoft.Practices.Unity;
using SimpleMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator
{
    public class UnityMigrationProvider : IMigrationProvider
    {
        private readonly IFoundation _foundation;

        public UnityMigrationProvider(IFoundation foundation)
        {
            _foundation = foundation ?? throw new ArgumentNullException(nameof(foundation));
        }

        public IEnumerable<MigrationData> LoadMigrations()
        {
            return _foundation.Container
                              .Registrations
                              .Where(rr => rr.RegisteredType == typeof(StencilDbMigrationBase))
                              .Select(AsMigrationData)
                              .ToList();
        }

        private MigrationData AsMigrationData(ContainerRegistration registration)
        {
            Type type = registration.MappedToType;
            var attribute = type.GetMigrationAttribute();
            return new MigrationData(attribute.Version, attribute.Description, type.GetTypeInfo());
        }
    }
}
