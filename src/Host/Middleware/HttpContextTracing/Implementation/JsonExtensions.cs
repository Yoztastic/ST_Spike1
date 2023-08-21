using System.Text;
using System.Text.Json;

namespace StorageSpike.Host.Middleware.HttpContextTracing.Implementation;

internal static class JsonExtensions
{
    internal static JsonDocument ExcludeFields(this JsonDocument token, ICollection<string> fields)
    {
        if (!fields.Any()) return token;

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        ExcludeFields(token.RootElement, fields, writer);
        writer.Flush();
        return JsonDocument.Parse(Encoding.UTF8.GetString(stream.ToArray()));
    }

    private static void ExcludeFields(JsonElement element, ICollection<string> fields, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    if (!fields.Contains(property.Name))
                    {
                        writer.WritePropertyName(property.Name);
                        ExcludeFields(property.Value, fields, writer);
                    }
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    ExcludeFields(item, fields, writer);
                }
                writer.WriteEndArray();
                break;
            case JsonValueKind.Undefined:
            case JsonValueKind.String:
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                element.WriteTo(writer);
                break;
        }
    }

    internal static JsonDocument Mask(this JsonDocument token, ICollection<string> maskNames)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        Mask(token.RootElement, maskNames, writer);
        writer.Flush();
        return JsonDocument.Parse(Encoding.UTF8.GetString(stream.ToArray()));
    }
    private static void Mask(JsonElement element, ICollection<string> maskNames, Utf8JsonWriter writer)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WritePropertyName(property.Name);
                    if (maskNames.Contains(property.Name))
                    {
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            writer.WriteStartArray();
                            foreach (var _ in property.Value.EnumerateArray())
                            {
                                writer.WriteStringValue("***");
                            }
                            writer.WriteEndArray();
                        }
                        else
                        {
                            writer.WriteStringValue("***");
                        }
                    }
                    else
                    {
                        Mask(property.Value, maskNames, writer);
                    }
                }
                writer.WriteEndObject();
                break;
            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    Mask(item, maskNames, writer);
                }
                writer.WriteEndArray();
                break;
            case JsonValueKind.Undefined:
            case JsonValueKind.String:
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                element.WriteTo(writer);
                break;
        }
    }
}
