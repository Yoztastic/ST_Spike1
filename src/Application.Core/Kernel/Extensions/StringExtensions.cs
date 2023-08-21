namespace StorageSpike.Application.Core.Kernel.Extensions;

public static class StringExtensions
{
    private static readonly char[] _unmaskedChars = { '@', '.', '_', '-', ' ' };
    private const int _minimumMaskingLevel = 2;

    public static string? Masked(this string? s, int level = 3, char maskingChar = '*')
    {
        return s.IsNullOrEmpty() ? s : new string(ToMaskedArray(s!, Math.Max(_minimumMaskingLevel, level), maskingChar));
    }

    public static string Join(this IEnumerable<string> join, string separator = ",") => string.Join( separator, join);

    private static char[] ToMaskedArray(string s, int level, char maskingChar)
    {
        return s.Select((c, i) => i % level == 0 || _unmaskedChars.Contains(c) ? c : maskingChar).ToArray();
    }
}
