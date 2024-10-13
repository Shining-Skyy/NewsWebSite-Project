using Domain.Roles;
using Domain.Users;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IdentityConfigs
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration["ConnectionString:SqlServer"];
            services.AddDbContext<IdentityDataBaseContext>(option => option.UseSqlServer(connection));

            services.AddIdentity<User, Role>().AddEntityFrameworkStores<IdentityDataBaseContext>()
                .AddDefaultTokenProviders().AddRoles<Role>().AddErrorDescriber<CustomIdentityError>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            return services;
        }
    }
}
