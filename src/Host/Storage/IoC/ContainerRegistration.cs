namespace StorageSpike.Host.Storage.IoC;

internal static class ContainerRegistration {
    internal static IServiceCollection AddStorageSpecificServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
