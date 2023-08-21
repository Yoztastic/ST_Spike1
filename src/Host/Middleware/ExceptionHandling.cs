namespace StorageSpike.Host.Middleware;

internal static class ExceptionHandling
{
    // ReSharper disable once UnusedMethodReturnValue.Local - Fluent Style
    internal static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        // todo configure exception handling conversions to errors here
        return app;
    }

}
