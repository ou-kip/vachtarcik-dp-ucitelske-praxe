using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuthorizationService.Swagger
{
    /// <summary>
    /// The options for swagger
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// .ctor for ConfigureSwaggerOptions
        /// </summary>
        /// <param name="provider"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configures the swagger
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        /// <summary>
        /// Creates the api version informations
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "Authorization Service",
                Version = description.ApiVersion.ToString(),
                Description = "REST API for authorization and authentication of the users."
            };
            return info;
        }
    }
}
