using StorageSpike.Host.IoC;

namespace StorageSpike.Host.Middleware.Headers;

internal static class Headers
{
    internal static IApplicationBuilder UseHttpHeaderPassthrough(this IApplicationBuilder app) =>
        app.UseMiddleware(typeof(HeaderMiddleware), DefaultHeaders.All);

    public static readonly string[] Mandatory = { DefaultHeaders.UserAgent, DefaultHeaders.ConversationId };
    internal static IApplicationBuilder UseEnsureMandatoryHeaders(this IApplicationBuilder app) =>
        app.UseMiddleware(typeof(MandatoryHeaderMiddleware), new object[] { Mandatory });

    internal static IApplicationBuilder UseHeaderTelemetry(this IApplicationBuilder app) =>
        app.UseMiddleware(typeof(HeaderTelemetryMiddleware));
}
