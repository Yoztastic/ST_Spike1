using StorageSpike.Host.Common.Logging;

namespace StorageSpike.Host.Middleware.HttpContextTracing;

internal static class RequestTracing
{
    private static readonly string[] MaskedHeaders = { "Authorization" };

    private static readonly string[] FieldsToMask =
    {
        "properties.trace.Request.Body.password",
        "password"
    };

    // ReSharper disable once UnusedMethodReturnValue.Local - Fluent Style
    internal static IApplicationBuilder UseMaskedRequestTracing(this IApplicationBuilder app)
    {
        var httpContextTraceLoggers = app.ApplicationServices.GetServices<IHttpContextTracer>();

        Predicate<HttpContext>[] exclusions =
        {
            context => context.Request.Path.StartsWithSegments(new PathString("/ping")),
            context => context.Request.Path.StartsWithSegments(new PathString("/diagnostics/healthcheck"))
        };

        return app.AddHttpContextTracing(httpContextTraceLoggers.ToArray())
            .WithFieldMasking(FieldsToMask.Concat(MaskedHeaders).ToArray())
            .WithExclusions(exclusions)
            .Use();
    }
}
