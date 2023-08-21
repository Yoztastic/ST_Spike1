using System.Diagnostics;

namespace StorageSpike.Application.Core.Kernel.Extensions;

public static class EnumerableExtensions
{
    [DebuggerStepThrough]
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? list)
    {
        return list == null || !list.Any();
    }

    public static T[] AsArray<T>(this T item)
    {
        return item.AsEnumerable().ToArray();
    }

    public static IEnumerable<T> AsEnumerable<T>(this T item)
    {
        if (item != null)
            yield return item;
    }
}
