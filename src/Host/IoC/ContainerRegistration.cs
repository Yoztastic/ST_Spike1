using Infrastructure.Common;
using StorageSpike.Application.Core;
using StorageSpike.Application.Core.Kernel;
using StorageSpike.Host.Middleware;
using StorageSpike.Host.Middleware.Headers;
using StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

namespace StorageSpike.Host.IoC;

internal static class ContainerRegistration {
    internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITransactionContext,HttpTransactionContext>();
        services.AddSingleton<ITelemetry, Telemetry>();
        services.AddSingleton<ITelemetryReporter, NewRelicTelemetryReporter>();
        services.AddSingleton<IHeaderService, SingletonHeaderService>();
        services.AddSingleton<ITelemetryReporter, NewRelicTelemetryReporter>();
        services.Configure<TogglesConfig>(configuration.GetSection(nameof(TogglesConfig)));
        services.AddScoped<DefaultHeadersHandler>();

        // this is how you can inject concrete dependencies in Infrastructure without actually allowing dependencies on Infrastructure in Host or Application
        /*services.Scan(s => s
            .FromAssemblies(Assembly.LoadFrom(@"./bin/Debug/net6.0/Infrastructure.dll"))
            .AddClasses(c => c.AssignableTo<IForecastProvider>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());*/
        return services;
    }
}
