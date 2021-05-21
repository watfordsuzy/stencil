using SimpleMigrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAndIndexMigrator
{
    internal static class MigrationExtensions
    {
        public static MigrationAttribute GetMigrationAttribute(this Type type)
            => type.GetCustomAttributes(typeof(MigrationAttribute), inherit: false)
                   .OfType<MigrationAttribute>()
                   .First();

        public static string GetMigrationName(this MigrationAttribute attribute)
            => GetMigrationName(attribute.Version, attribute.Description);

        public static string GetMigrationName(this MigrationData migrationData)
            => GetMigrationName(migrationData.Version, migrationData.Description);

        private static string GetMigrationName(long version, string description)
            => $"Migration-{version:0000}-{description}";
    }
}
