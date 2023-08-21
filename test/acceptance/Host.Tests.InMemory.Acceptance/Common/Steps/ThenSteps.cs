using System.Net;
using FluentAssertions;
using StorageSpike.Acceptance.Helpers;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.Steps;

public class ThenSteps
{
    public ThenSteps(HostTestContext context)
    {
        Context = context;
    }
    public void TheResponseCodeShouldBe(IApiResponse response, HttpStatusCode statusCode)
    {
        response.StatusCode.Should().Be(statusCode);
    }

    private HostTestContext Context { get;  }
}
