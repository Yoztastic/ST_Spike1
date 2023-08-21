namespace Infrastructure.Common;

public class ContextAccessor : IContextAccessor
{
    private readonly ITransactionContext _transactionContext;

    public ContextAccessor(ITransactionContext transactionContext) => _transactionContext = transactionContext;

    public string ConversationId => _transactionContext.Headers.TryGetValue(HeaderNames.ConversationId, out var conversationId)
        ? conversationId
        : ConversationStarter.ConversationId;

    public string? ContextIdentity  => _transactionContext.Headers.TryGetValue(HeaderNames.ContextIdentity, out var contextUri)
        ? contextUri
        : null;
}
