using System.Text.Json;

namespace Infrastructure.Converters;
public static class SerializerExtensions
{
    private static bool IsPropertyName(this ref Utf8JsonReader reader, string propertyName) =>
        reader.Read() && reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == propertyName;

    public static string? FindAndRetrieveStringProperty(this ref Utf8JsonReader reader, string propertyName)
    {
        while (!reader.IsPropertyName(propertyName) && reader.TokenType != JsonTokenType.EndObject) reader.Read();

        return !reader.Read() || reader.TokenType != JsonTokenType.String
            ? throw new JsonException($"Cant find instance of {propertyName}")
            : reader.GetString();
    }
}
