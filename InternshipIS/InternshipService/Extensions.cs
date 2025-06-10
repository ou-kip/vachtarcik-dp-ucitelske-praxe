using Core.Messaging.Configuration;
using Core.Services;
using InternshipService.Configuration;
using InternshipService.Consumers;
using InternshipService.Database;
using InternshipService.Database.EntityConfigurations;
using InternshipService.Database.Repositories;
using InternshipService.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace InternshipService
{
    /// <summary>
    /// The extensions for Authorization service collection
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds the client settings to the options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddClientSettings(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<ClientOptions>(options => configuration.GetSection(nameof(ClientOptions)).Bind(options));
            return services;
        }

        /// <summary>
        /// Adds the versioning
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }

        /// <summary>
        /// Adds the swagger configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        /// <summary>
        /// Use and enable all the extensions and configurations about swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseCustomSwaggerUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    var descriptions = app.DescribeApiVersions();
                    foreach (var description in descriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

            return app;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<MessagingOptions>(options => configuration.GetSection(nameof(MessagingOptions)).Bind(options));

            services.AddMassTransit(bus =>
            {
                bus.AddConsumer<InternshipPersonCreateConsumer>();
                bus.AddConsumer<InternshipPersonDeleteConsumer>();

                bus.SetKebabCaseEndpointNameFormatter();
                bus.UsingRabbitMq((context, configurator) =>
                {
                    var options = context.GetRequiredService<IOptions<MessagingOptions>>().Value;
                    configurator.Host(new Uri(options.Host!), cfg =>
                    {
                        cfg.Username(options.UserName);
                        cfg.Username(options.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }

        /// <summary>
        /// Adds the core stuff for authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreAuthentication(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var sp = services.BuildServiceProvider();
                var jtwOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jtwOptions.Audience,
                    ValidIssuer = jtwOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jtwOptions.Key))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Bearer"];
                        return Task.CompletedTask;
                    }
                };
            });
            return services;
        }

        /// <summary>
        /// Add the authorization db context
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoriesWithDbContext(this IServiceCollection services, IConfigurationManager configuration)
        {
            //configurations
            services.AddSingleton<IInternshipCompanyRelativeEntityConfiguration, InternshipCompanyRelativeEntityConfiguration>();
            services.AddSingleton<IInternshipEntityConfiguration, InternshipEntityConfiguration>();
            services.AddSingleton<IInternshipStudentEntityConfiguration, InternshipStudentEntityConfiguration>();
            services.AddSingleton<IInternshipTaskEntityConfiguration, InternshipTaskEntityConfiguration>();
            services.AddSingleton<IInternshipTaskFileEntityConfiguration, InternshipTaskFileEntityConfiguration>();
            services.AddSingleton<IInternshipTeacherEntityConfiguration, InternshipTeacherEntityConfiguration>();
            services.AddSingleton<IInternshipLinkEntityConfiguration, InternshipLinkEntityConfiguration>();
            services.AddSingleton<IInternshipCategoryEntityConfiguration, InternshipCategoryEntityConfiguration>();
            services.AddSingleton<IInternshipTaskLinkEntityConfiguration, InternshipTaskLinkEntityConfiguration>();
            services.AddSingleton<IInternshipTaskSolutionEntityConfiguration, InternshipTaskSolutionEntityConfiguration>();
            services.AddSingleton<IInternshipTaskSolutionFileEntityConfiguration, InternshipTaskSolutionFileEntityConfiguration>();

            services.Configure<DatabaseOptions>(options => configuration.GetSection(nameof(DatabaseOptions)).Bind(options));
            services.AddDbContext<InternshipDbContext>((serviceProvider, options) =>
            {
                var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>();
                options.UseSqlServer(dbOptions.Value.ConnectionString);
            });

            //repositories
            services.AddScoped<IInternshipRepository, InternshipRepository>();
            services.AddScoped<IInternshipStudentRepository, InternshipStudentRepository>();
            services.AddScoped<IInternshipTeacherRepository, InternshipTeacherRepository>();
            services.AddScoped<IInternshipCompanyRelativeRepository, InternshipCompanyRelativeRepository>();
            services.AddScoped<IInternshipTaskRepository, InternshipTaskRepository>();
            services.AddScoped<IInternshipLinkRepository, InternshipLinkRepository>();
            services.AddScoped<IInternshipCategoryRepository, InternshipCategoryRepository>();
            services.AddScoped<IInternshipTaskLinkRepository, InternshipTaskLinkRepository>();
            services.AddScoped<IInternshipTaskFileRepository, InternshipTaskFileRepository>();
            services.AddScoped<IInternshipTaskSolutionRepository, InternshipTaskSolutionRepository>();
            services.AddScoped<IInternshipTaskSolutionFileRepository, InternshipTaskSolutionFileRepository>();

            return services;
        }

        /// <summary>
        /// Add the authorization db context
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            return services;
        }
        /// <summary>
        /// Applies the migrations
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static IServiceProvider ApplyDbMigrations(this IServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<InternshipDbContext>();
                db.Database.Migrate();
            }

            return sp;
        }
    }
}
