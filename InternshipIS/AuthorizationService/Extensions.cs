using AuthorizationService.Configuration;
using AuthorizationService.Services;
using AuthorizationService.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace AuthorizationService
{
    /// <summary>
    /// The extensions for Authorization service collection
    /// </summary>
    public static class Extensions
    {
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
        /// Adds the core stuff for authentication
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoreAuthentication(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

            services.AddScoped<IAuthorizationService, Services.AuthorizationService>();
            services.AddScoped<IClaimsProcessor, ClaimsProcessor>();
            services.AddScoped<ICookieProcessor, CookieProcessor>();

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
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //    options.ExpireTimeSpan = TimeSpan.FromDays(5);
            //    options.SlidingExpiration = true; 
            //});

            return services;
        }
    }

    /// <summary>
    /// The extensions for configuring the swagger
    /// </summary>
    public static class SwaggerConfigurationExtensions
    {
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

        /// <summary>
        /// Adds the memory cache
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomMemoryCache(this IServiceCollection services) 
        {
            services.AddMemoryCache();
            services.AddSingleton<ITokenMemoryCache, TokenMemoryCache>();
            services.AddSingleton<IResetPasswordTokenMemoryCache, ResetPasswordTokenMemoryCache>();
            services.AddSingleton<IConfirmnTokenMemoryCache, ConfirmnTokenMemoryCache>();

            return services;
        }
    }
}
