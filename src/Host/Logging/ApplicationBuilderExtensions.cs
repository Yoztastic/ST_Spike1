namespace StorageSpike.Host.Logging;

public static class ApplicationBuilderExtensions
{
    private static readonly string[] DiagnosticPaths = { "/diagnostics", "/swagger" };
    public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        => UseLogging(
            builder,
            context => !DiagnosticPaths.Any(c => context.Request.Path.StartsWithSegments(c)));

    private static IApplicationBuilder UseLogging(this IApplicationBuilder builder, Func<HttpContext, bool> diagnosticsPredicate)
    {
        builder.UseMiddleware<LoggingMiddleware>();
        builder.UseWhen(
            diagnosticsPredicate,
            branchedBuilder => branchedBuilder.UseMiddleware<DiagnosticsMiddleware>());
        return builder;
    }
}
