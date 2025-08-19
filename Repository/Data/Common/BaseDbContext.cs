using Domain.Common;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Domain.Base.Enums;

namespace Infrastructure.Data.Common
{
    public abstract class BaseDbContext : DbContext
    {
        protected readonly ConnectionSettings _connection;

        protected BaseDbContext(IOptions<AppSettings> settings, string connectionName, SgbdType expectedType)
        {
            _connection = settings.Value.Connections.First(c => c.Name == connectionName && c.Sgbd == expectedType);
        }

        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
            transaction.Dispose();
        }

        public void RollbackTransaction(IDbContextTransaction transaction)
        {
            transaction.Rollback();
            transaction.Dispose();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBaseEntityRelationships(modelBuilder);
            ConfigureColumnMappings(modelBuilder);
            ConfigureIgnoreUpdate(modelBuilder);
        }

        protected void ConfigureBaseEntityRelationships(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType) ||
                    typeof(IEntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasKey("Id");
                }
            }
        }

        protected void ConfigureColumnMappings(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;
                var properties = clrType.GetProperties();

                foreach (var property in properties)
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    if (columnAttribute != null)
                    {
                        modelBuilder.Entity(clrType)
                            .Property(property.Name)
                            .HasColumnName(columnAttribute.Name);
                    }
                }
            }
        }

        protected void ConfigureIgnoreUpdate(ModelBuilder modelBuilder)
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