
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using StorageSpike.Acceptance.Helpers;
using StorageSpike.Host.Settings;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common.Logging;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common.Steps;
using Xunit.Abstractions;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common;

public abstract class AcceptanceTestBase : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;
    private ApiClient ApiClient { get; }
    protected ServiceSettings AppSettings { get; }
    protected HostTestContext Context { get;}
    protected IServiceProvider ServiceProvider { get; }
    protected Uri BaseAddress { get; }

    protected ThenSteps Then { get; }
    protected WhenSteps When { get; }

    protected InMemoryGivenSteps Given { get; }

    protected AcceptanceTestBase(ITestOutputHelper testOutputHelper)
    {
        _factory = new InMemoryApplicationFactory()
            .WithWebHostBuilder(b =>
                b.ConfigureLogging((h, l) =>
                    l.AddXunit(testOutputHelper)));

        var httpClient = _factory.CreateClient();

        ApiClient = ApiClientFactory.For(httpClient);

        BaseAddress = httpClient.BaseAddress!;

        ServiceProvider = _factory.Services.CreateScope().ServiceProvider;

        AppSettings = ServiceProvider.GetRequiredService<IOptions<ServiceSettings>>().Value;

        Context = ServiceProvider.GetRequiredService<HostTestContext>();
        Context.ApiClient = ApiClient;

        Then = new ThenSteps(Context);
        When = new WhenSteps(Context);

        Given = ServiceProvider.GetService<InMemoryGivenSteps>() ?? throw new InvalidOperationException($"Cant resolve {nameof(InMemoryGivenSteps)}");
    }

    void IDisposable.Dispose()
    {
        ApiClient?.Dispose();
        _factory.Dispose();
    }
}
