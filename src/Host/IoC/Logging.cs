using Destructurama;
using Infrastructure.Common;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using StorageSpike.Application.Core;
using StorageSpike.Application.Core.Kernel;
using StorageSpike.Host.Common.Logging;
using StorageSpike.Host.Logging;

namespace StorageSpike.Host.IoC;

internal static class Logging
{
    internal static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        services.AddSingleton<IHttpContextTracer, HttpContextTraceLogger>();
        services.AddScoped<IContextAccessor, ContextAccessor>();

        return services.AddLoggingWithWorkingJsonNetTypes(configuration, options =>
        {
            options.LogOriginalIp = false;
            options.ConsoleAsJson = true;
            options.WriteToConsole = true;
            options.ExtraProperties = ReportingConstants.All;
        });
    }

    private static IServiceCollection AddLoggingWithWorkingJsonNetTypes(this IServiceCollection services, IConfiguration configuration, Action<LoggingConfiguration> configureOptions) =>
        services
            .AddLoggingWithWorkingJsonNetTypes(configuration)
            .Configure(configureOptions);

    private static IServiceCollection AddLoggingWithWorkingJsonNetTypes(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<LoggingConfiguration>(configuration)
            .AddSingleton(s =>
            {
                var loggerConfiguration = new LoggerConfiguration();
                var telemetry = s.GetRequiredService<ITelemetry>();
                loggerConfiguration.Configure(
                    s.GetRequiredService<IConfiguration>(),
                    s.GetRequiredService<IOptions<LoggingConfiguration>>().Value);
                loggerConfiguration.Destructure.JsonNetTypes();

                var logger = loggerConfiguration.CreateLogger();
                return (ILoggerFactory)new TelemetryAwareLoggerFactory(new SerilogLoggerFactory(logger, true), telemetry);
            });
}