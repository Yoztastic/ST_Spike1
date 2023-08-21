using Microsoft.AspNetCore.Mvc.Testing;
using StorageSpike.Host.Tests.InMemory.Acceptance.Stubs;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common;

public class InMemoryApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
            {
                services.RegisterStubs();
            })
            .UseEnvironment("Development");
    }
}
