using Application.Interfaces.Contexts;
using Domain.Attributes;
using Domain.Banners;
using Domain.Categorys;
using Domain.Comments;
using Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostFavorite> PostFavorites { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Iterate through all entity types in the model
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Check if the entity type has the AuditableAttribute
                if (entityType.ClrType.GetCustomAttributes(typeof(AuditableAttribute), true).Length > 0)
                {
                    modelBuilder.Entity(entityType.Name).Property<DateTime>("InsertTime").HasDefaultValue(DateTime.Now);
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("UpdateTime");
                    modelBuilder.Entity(entityType.Name).Property<DateTime?>("RemoveTime");
                    modelBuilder.Entity(entityType.Name).Property<bool>("IsRemove").HasDefaultValue(false);
                }
            }

            // Apply global query filters to exclude soft-deleted entities
            modelBuilder.Entity<CategoryType>().HasQueryFilter(m => EF.Property<bool>(m, "IsRemove") == false);
            modelBuilder.Entity<Post>().HasQueryFilter(m => EF.Property<bool>(m, "IsRemove") == false);
            modelBuilder.Entity<PostFavorite>().HasQueryFilter(m => EF.Property<bool>(m, "IsRemove") == false);
            modelBuilder.Entity<Banner>().HasQueryFilter(m => EF.Property<bool>(m, "IsActive") == true);
            modelBuilder.Entity<Comment>().HasQueryFilter(m => EF.Property<bool>(m, "IsRemove") == false);
        }

        public override int SaveChanges()
        {
            // Get entries that are added, modified, or deleted
            var returnEntries = ChangeTracker.Entries().Where(p =>
            p.State == EntityState.Added ||
            p.State == EntityState.Modified ||
            p.State == EntityState.Deleted);

            foreach (var item in returnEntries)
            {
                // Find the entity type for the current item
                var entityType = item.Context.Model.FindEntityType(item.Entity.GetType());
                if (entityType != null)
                {
                    // Find properties related to auditing
                    var inserted = entityType.FindProperty("InsertTime");
                    var updated = entityType.FindProperty("UpdateTime");
                    var Removed = entityType.FindProperty("RemoveTime");
                    var IsRemoved = entityType.FindProperty("IsRemove");

                    // If the entity is being added, set InsertTime to now
                    if (item.State == EntityState.Added & inserted != null)
                    {
                        item.Property("InsertTime").CurrentValue = DateTime.Now;
                    }

                    // If the entity is being modified, set UpdateTime to now
                    if (item.State == EntityState.Modified & inserted != null)
                    {
                        item.Property("UpdateTime").CurrentValue = DateTime.Now;
                    }

                    // If the entity is being deleted, set RemoveTime and mark as removed
                    if (item.State == EntityState.Deleted & inserted != null && IsRemoved != null)
                    {
                        item.Property("RemoveTime").CurrentValue = DateTime.Now;
                        item.Property("IsRemove").CurrentValue = true;
                        item.State = EntityState.Modified;
                    }
                }
            }
            return base.SaveChanges();
        }
    }
}
