using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using JsonMasking.Services;

namespace JsonMasking
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSensitiveFieldAuditing(this IServiceCollection services)
        {
            //services.AddOptions();
            _ = services.AddSingleton<ISensitiveAuditingFields, SensitiveAuditingFields>();
            _ = services.AddScoped<ISanitizeDataService, SanitizeDataService>();
            _ = services.AddScoped<SensitiveAuditingFieldsContext>();
            // services.PostConfigure(options);
            // if (options != null)
            //     services.Configure(options);

            // services.TryAddSingleton<IConfigureOptions<SensitiveAuditingFieldsOption>, SensitiveAuditingFieldsOptionSetup>();

            return services;
        }

        public static IApplicationBuilder UseSensitiveFieldAuditing(this IApplicationBuilder app)
        {
            app.UseMiddleware<IpWhiteListingMiddleware>();

            return app;
        }
    }
}