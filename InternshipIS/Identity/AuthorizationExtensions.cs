using Identity.Configuration;
using Identity.Database.Entities;
using Identity.Database.EntityConfigurations;
using Identity.Database.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity
{
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Adds the authorization repositories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        /// <summary>
        /// Add the authorization db context
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationWithDbContext(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddScoped<IRoleEntityConfiguration, RoleEntityConfiguration>();
            services.AddScoped<IUserEntityConfiguration, UserEntityConfiguration>();

            services.Configure<AuthorizationOptions>(options =>
            {
                configuration.GetSection(nameof(AuthorizationOptions)).Bind(options);

                var sqlHost = Environment.GetEnvironmentVariable("SQL_HOST");
                if (!string.IsNullOrEmpty(sqlHost))
                {
                    options.ConnectionString = options.ConnectionString.Replace("SQL_HOST_PLACEHOLDER", sqlHost);
                }
            });

            services.AddDbContext<AuthorizationDbContext>((serviceProvider, options) =>
            {
                var authorizationOptions = serviceProvider.GetRequiredService<IOptions<AuthorizationOptions>>();
                options.UseSqlServer(authorizationOptions.Value.ConnectionString);
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthorizationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Applies the migrations related to the identity db context
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IServiceProvider ApplyAuthorizationDbMigrations(this IServiceProvider sp) 
        {
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
                db.Database.Migrate();
            }

            return sp;
        }
    }
}
