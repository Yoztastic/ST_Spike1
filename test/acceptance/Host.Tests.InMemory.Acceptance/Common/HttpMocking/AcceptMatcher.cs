using RichardSzalay.MockHttp.Matchers;

namespace StorageSpike.Host.Tests.InMemory.Acceptance.Common.HttpMocking;

public class AcceptMatcher : CustomMatcher
{
    public AcceptMatcher(string accept) : base(r => r.Headers.Accept.ToString() == accept){}
}
