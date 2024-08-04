using Application.Interfaces.Contexts;
using Domain.Attributes;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertTime").HasDefaultValue(DateTime.Now);
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemove").HasDefaultValue(false);
                }
            }
        }

        public override int SaveChanges()
        {
            var returnEntries = ChangeTracker.Entries().Where(p =>
            p.State == EntityState.Added ||
            p.State == EntityState.Modified ||
            p.State == EntityState.Deleted);

            foreach (var item in returnEntries)
            {
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());

                if (entityType != null)
                {
                    var inserted = entityType.FindProperty("InsertTime");
                    var updated = entityType.FindProperty("UpdateTime");
                    var Removed = entityType.FindProperty("RemoveTime");
                    var IsRemoved = entityType.FindProperty("IsRemove");
                    if (item.State == EntityState.Added & inserted != null)
                    {
                        item.Property("InsertTime").CurrentValue = DateTime.Now;
                    }
                    if (item.State == EntityState.Modified & inserted != null)
                    {
                        item.Property("UpdateTime").CurrentValue = DateTime.Now;
                    }
                    if (item.State == EntityState.Deleted & inserted != null && IsRemoved != null)
                    {
                        item.Property("RemoveTime").CurrentValue = DateTime.Now;
                        item.Property("IsRemove").CurrentValue = true;
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
