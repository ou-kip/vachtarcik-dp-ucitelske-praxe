using Core.Messaging.Configuration;
using MassTransit;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NotificationService.Configuration;
using NotificationService.Consumers;
using NotificationService.Service;
using NotificationService.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NotificationService
{
    /// <summary>
    /// The extensions for NotificationService service collection
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
        /// Adds emailing
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailing(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<EmailingOptions>(options => configuration.GetSection(nameof(EmailingOptions)).Bind(options));
            services.AddScoped<ISmtpService, SmtpService>();

            return services;
        }

        //TODO rework
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.Configure<MessagingOptions>(options => configuration.GetSection(nameof(MessagingOptions)).Bind(options));

            services.AddMassTransit(bus =>
            {
                bus.AddConsumer<RegistrationConsumer>();
                bus.AddConsumer<PasswordResetConsumer>();
                bus.AddConsumer<CommonEmailConsumer>();
                bus.AddConsumer<ConfirmnAccountConsumer>();

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
        /// Adds and configures the http logging
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHttpLogging(this IServiceCollection services)
        {
            return services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add(HeaderNames.Accept);
                logging.RequestHeaders.Add(HeaderNames.ContentType);
                logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
                logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
                logging.RequestHeaders.Add(HeaderNames.ContentLength);

                logging.MediaTypeOptions.AddText("application/json");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
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
    }

}
