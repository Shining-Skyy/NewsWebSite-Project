using Application.Interfaces.Contexts;
using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class IdentityDataBaseContext : IdentityDbContext<User, Role, string>, IIdentityDatabaseContext
    {
        public IdentityDataBaseContext(DbContextOptions<IdentityDataBaseContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser<string>>().ToTable("Users", "identity");
            modelBuilder.Entity<IdentityRole<string>>().ToTable("Roles", "identity");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
        }
    }
}
