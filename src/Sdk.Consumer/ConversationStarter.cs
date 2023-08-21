public class ConversationStarter : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("ConversationId",Guid.NewGuid().ToString());
        return base.SendAsync(request, cancellationToken);
    }
}
