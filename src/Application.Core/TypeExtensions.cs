using System.Reflection;

namespace StorageSpike.Application.Core;

public static class TypeExtensions
{
    public static IList<string> GetAllStringFields(this Type me) =>
        me
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fi => fi.GetValue(null))
            .OfType<string>()
            .ToList();
}
