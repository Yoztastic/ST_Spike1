namespace Infrastructure.Common;

public interface IContextAccessor
{
    /// <summary>
    /// Traceable identifier used across estate
    /// </summary>
    string ConversationId { get; }

    /// <summary>
    /// context information of caller ie who are they what role
    /// </summary>
    string? ContextIdentity { get; }
}
