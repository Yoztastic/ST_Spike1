using RichardSzalay.MockHttp;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;

public class AlwaysTrueMatcher : IMockedRequestMatcher
{
    public bool Matches(HttpRequestMessage message) => true;
}
