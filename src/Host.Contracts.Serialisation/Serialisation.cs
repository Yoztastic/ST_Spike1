using System.Text.Json;
using System.Text.Json.Serialization;

namespace Host.Contracts.Serialisation;

public static class Serialisation
{
    public static JsonSerializerOptions Options { get; }

    static Serialisation()
    {
        Options = new JsonSerializerOptions(JsonSerializerDefaults.General)
        {
            Converters = { new DateOnlyJsonConverter(),new JsonStringEnumConverter()},
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

    }
}