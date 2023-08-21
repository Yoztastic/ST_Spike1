using AutoFixture;

using StorageSpike.Acceptance.Helpers;

namespace StorageSpike.Host.Tests.EndToEnd.Acceptance;

public abstract class AcceptanceTestBase : IDisposable
{
    private ApiClient ApiClient { get; set; }
    private Fixture Fixture { get; set; }
    protected AppSettings AppSettings { get; private set; }
    protected HostTestContext Context { get; private set; }

    protected AcceptanceTestBase()
    {
        Fixture = new Fixture();

        AppSettings = ConfigurationService.Load();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(string.Format(AppSettings.ExternalAddress,ConfigurationService.Env))
        };

        ApiClient = ApiClientFactory.For(httpClient);

        Context = new HostTestContext{ApiClient = ApiClient};
    }

    public void Dispose()
    {
        ApiClient.Dispose();
    }
}
