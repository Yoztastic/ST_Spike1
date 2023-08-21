using StorageSpike.Application.Core;

namespace StorageSpike.Host.IoC;

internal static class DefaultHeaders
{
    static DefaultHeaders() => All = typeof(DefaultHeaders).GetAllStringFields();

    public const string ConversationId = nameof(ConversationId);
    public const string UserAgent = nameof(UserAgent);
    public const string ContextIdentity = nameof(ContextIdentity);
    public static IList<string> All { get; }
}
