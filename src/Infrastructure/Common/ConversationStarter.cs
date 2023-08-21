namespace Infrastructure.Common;

public static class ConversationStarter
{
    public static string ConversationId => $"StorageSpike-{DateTime.Now:yyyyMMddHHmmss}{NewAbsoluteHashCode()}";

    private static int NewAbsoluteHashCode()
    {
        var hash = Guid.NewGuid().GetHashCode();
        return Math.Abs(hash);
    }
}
