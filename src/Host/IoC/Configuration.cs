using StorageSpike.Host.Settings;

namespace StorageSpike.Host.IoC;

internal static class Configuration
{
    internal static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<ServiceSettings>(configuration.GetSection("Service"));
        return services;
    }
}
