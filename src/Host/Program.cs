using Microsoft.FeatureManagement;
using StorageSpike.Host;
using StorageSpike.Host.IoC;
using StorageSpike.Host.Logging;
using StorageSpike.Host.Middleware;
using StorageSpike.Host.Middleware.Headers;
using StorageSpike.Host.Middleware.HttpContextTracing;
using StorageSpike.Host.Storage.IoC;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSwagger()
    .AddConfiguration(builder.Configuration)
    .AddLogging(builder.Configuration)
    .AddFluentValidation()
    .AddServices(builder.Configuration)
    .AddStorageSpecificServices(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(SerialisationConfigurationExtensions.Configure);

builder.Services
    .AddFeatureManagement(builder.Configuration.GetSection("TogglesConfig"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks()
    .UseLogging()
    .UseHttpsRedirection()
    .UseHttpHeaderPassthrough()
    .UseMaskedRequestTracing()
    .UseExceptionHandling()
    .UseEnsureMandatoryHeaders()
    .UseHeaderTelemetry()
    .UseMiddleware<TelemetryReportingMiddleware>();

app.MapControllers();

app.Run();

namespace StorageSpike.Host
{
    /// <summary>
    /// Used by the in-memory acceptance tests
    /// </summary>
    public partial class Program {}
}
