using StorageSpike.Host.Common.Logging;
using StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

namespace StorageSpike.Host.Middleware.HttpContextTracing;

internal static class ApplicationBuilderExtensions
{
    internal static TracingConfigurationBuilder<HttpContext, IApplicationBuilder> AddHttpContextTracing(this IApplicationBuilder applicationBuilder, params IHttpContextTracer[] tracers)
    {
        var tracingActions = tracers
            .Select(tracer => (Action<HttpContextTrace>) tracer.Trace)
            .ToArray();

        return AddHttpContextTracing(applicationBuilder, tracingActions);
    }

    private static TracingConfigurationBuilder<HttpContext, IApplicationBuilder> AddHttpContextTracing(this IApplicationBuilder applicationBuilder, params Action<HttpContextTrace>[] tracingActions)
    {
        var configurationBuilder = new TracingConfigurationBuilder<HttpContext, IApplicationBuilder>(configuration =>
            applicationBuilder.UseMiddleware<HttpContextTracingMiddleware>(configuration, tracingActions)
        );

        return configurationBuilder;
    }
}
