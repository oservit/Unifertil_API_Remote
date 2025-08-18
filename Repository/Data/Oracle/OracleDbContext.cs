using Domain.Base;
using Domain.Base.Enums;
using Domain.Settings;
using Infrastructure.Data.Base;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Infrastructure.Data.Oracle
{
    public class OracleDbContext : BaseDbContext
    {
        public OracleDbContext(IOptions<AppSettings> settings, string connectionName = "Oracle")
            : base(settings, connectionName, SgbdType.Oracle)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(_connection.ConnectionString);

#if DEBUG
                optionsBuilder
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(new SqlLoggingInterceptor());
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureSequences(modelBuilder);
            ConfigureConversions(modelBuilder);
            ConfigureIgnoreUpdate(modelBuilder);
            modelBuilder.HasDefaultSchema(_connection.Schema);
        }

        private void ConfigureSequences(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey != null)
                {
                    var keyProperty = primaryKey.Properties.FirstOrDefault();
                    if (keyProperty != null)
                    {
                        var attribute = keyProperty.PropertyInfo?.GetCustomAttribute<SequenceAttribute>();
                        if (attribute != null)
                        {
                            modelBuilder.HasSequence<long>(attribute.SequenceName, schema: _connection.Schema);

                            modelBuilder.Entity(entityType.ClrType)
                                .Property(keyProperty.Name)
                                .ValueGeneratedOnAdd()
                                .HasDefaultValueSql($"{_connection.Schema}.{attribute.SequenceName}.NEXTVAL");
                        }
                    }
                }
            }
        }

        private void ConfigureConversions(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.ClrType.GetProperties())
                {
                    if (!property.CanWrite)
                        continue;

                    var isColumnMapped = Attribute.IsDefined(property, typeof(ColumnAttribute));
                    if (!isColumnMapped)
                        continue;

                    if (property.PropertyType == typeof(bool))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(property.Name)
                            .HasConversion(new ValueConverter<bool, int>(
                                v => v ? 1 : 0,
                                v => v == 1
                            ));
                    }
                    else if (property.PropertyType == typeof(bool?))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property(property.Name)
                            .HasConversion(new ValueConverter<bool?, int?>(
                                v => v.HasValue ? (v.Value ? 1 : 0) : (int?)null,
                                v => v.HasValue ? v.Value == 1 : (bool?)null
                            ));
                    }
                }
            }
        }

        private void ConfigureIgnoreUpdate(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    var clrProperty = property.PropertyInfo;
                    if (clrProperty != null && clrProperty.IsDefined(typeof(IgnoreOnUpdateAttribute), inherit: true))
                    {
                        property.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                    }
                }
            }
        }
    }
}