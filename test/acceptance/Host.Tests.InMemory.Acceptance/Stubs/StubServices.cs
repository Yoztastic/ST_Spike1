using RichardSzalay.MockHttp;
using StorageSpike.Acceptance.Helpers;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common.Steps;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Stubs;

public static class StubServices
{
    public static void RegisterStubs(this IServiceCollection services)
    {
        services.AddSingleton<HostTestContext>();
        services.AddSingleton<IHttpClientFactory,FakeHttpClientFactory>();
        services.AddSingleton<MockHttpMessageHandler>();
        services.AddSingleton<InMemoryGivenSteps>();
    }
}
