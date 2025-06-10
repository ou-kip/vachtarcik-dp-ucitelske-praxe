using Core.Messaging.Configuration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Messaging
{
    /// <summary>
    /// The extensions for messaging
    /// </summary>
    public static class MassTransitServiceExtensions
    {
        /// <summary>
        /// Adds the messaging via masstransit / rabbitmq
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfigurationManager configuration) 
        {
            services.Configure<MessagingOptions>(options => configuration.GetSection(nameof(MessagingOptions)).Bind(options));

            services.AddMassTransit(bus =>
            {
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
        /// Adds the consumers
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureConsumers"></param>
        /// <returns></returns>
        public static IServiceCollection AddConsumers(this IServiceCollection services, Action<IServiceCollection> configureConsumers)
        {
            configureConsumers(services);
            return services;
        }
    }
}
