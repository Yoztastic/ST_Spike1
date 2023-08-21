using System.Net;
using FluentAssertions;
using Xunit;
using StorageSpike.Acceptance.Helpers.TestContracts.Diagnostics;
using StorageSpike.Host.Tests.InMemory.Acceptance.Common;
using Xunit.Abstractions;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Features;

public class DiagnosticsFeature : AcceptanceTestBase
{
    public DiagnosticsFeature(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task GetDiagnostics()
    {
        var response = await When.IGet<ServiceInformationContract>("/diagnostics/healthcheck");

        Then.TheResponseCodeShouldBe(response, HttpStatusCode.OK);

        response.Content.ServiceName.Should().Be(AppSettings.ApplicationName);
        response.Content.ApplicationId.Should().Be(AppSettings.ApplicationId);
        response.Content.Slice.Should().Be(AppSettings.Slice);
        response.Content.Version.Should().Be(AppSettings.Version());
        response.Content.IsHealthy.Should().BeTrue();
    }
}
