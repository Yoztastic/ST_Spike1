namespace StorageSpike.Host.Middleware;

internal static class Swagger
{
    // ReSharper disable once UnusedMethodReturnValue.Local - Fluent Style
    internal static IApplicationBuilder UseSwagger(this IApplicationBuilder app) =>
        app.UseSwagger(c => { c.RouteTemplate = "swagger/{documentName}/swagger.json"; })
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StorageSpike V1");
                c.RoutePrefix = "docs";
            });
}
