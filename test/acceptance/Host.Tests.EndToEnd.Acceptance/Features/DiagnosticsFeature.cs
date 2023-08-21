using System.Net;
using FluentAssertions;
using StorageSpike.Acceptance.Helpers;
using TestStack.BDDfy;
using StorageSpike.Acceptance.Helpers.TestContracts.Diagnostics;
using Xunit;

namespace StorageSpike.Host.Tests.EndToEnd.Acceptance.Features;


public class DiagnosticsFeature : AcceptanceTestBase
{
    private IApiResponse<ServiceInformationContract>? _response;

    [Fact]
    public void GetDiagnostics()
    {
        this.When(_ => _.IPerformHealthCheck())
            .Then(_ => _.TheResponseContainsServiceInformation())
            .BDDfy();
    }

    private async Task IPerformHealthCheck()
    {
        _response = await Context.ApiClient.GetAsync<ServiceInformationContract>("/diagnostics/healthcheck");
    }

    private void TheResponseContainsServiceInformation()
    {
        _response.Should().NotBeNull();
        _response!.Content.ServiceName.Should().NotBeNull();
        _response.Content.ApplicationId.Should().NotBeNull();
        _response!.StatusCode.Should().Be(HttpStatusCode.OK);
        _response.Content.IsHealthy.Should().BeTrue();
    }
}
