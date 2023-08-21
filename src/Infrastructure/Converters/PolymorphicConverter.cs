using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using StorageSpike.Application.Core;

[assembly: InternalsVisibleTo("Infrastructure.UnitTests")]
namespace Infrastructure.Converters;
public class PolymorphicConverter<T> : JsonConverter<T> where T : ISerialisePolymorphically
{
    private static readonly Type type = typeof(T);
    private readonly IEnumerable<Type> _knownTypes;

    internal PolymorphicConverter(Type[] knownTypes)
    {
        EnsureKnownTypesAreDerivedFromT(knownTypes);
        _knownTypes = knownTypes;
    }

    private static void EnsureKnownTypesAreDerivedFromT(Type[] knownTypes)
    {
        foreach (var knownType in knownTypes)
            if (!type.IsAssignableFrom(knownType))
                throw new ArgumentException($"{type.Name} is not assignable from {knownType.Name}", nameof(knownTypes));
    }

    public override bool CanConvert(Type typeToConvert) =>
        typeof(T).IsAssignableFrom(typeToConvert);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var typeReader = reader;

        var typeDiscriminator = typeReader.FindAndRetrieveStringProperty(nameof(ISerialisePolymorphically.TypeDiscriminator));

        foreach (var knownType in _knownTypes)
            if (knownType.Name == typeDiscriminator)
                return (T)JsonSerializer.Deserialize(ref reader, knownType, GetOptionsCopy(options))!;

        throw
            new JsonException(
                $"No deserialisation support for {typeDiscriminator}");
    }

    private JsonSerializerOptions GetOptionsCopy(JsonSerializerOptions optionsCopy)
    {
        var optionCopy = new JsonSerializerOptions(optionsCopy);

        optionCopy.Converters.Remove(this);
        return optionCopy;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {

        foreach (var knownType in _knownTypes)
        {
            if (value.GetType() != knownType) continue;
            writer.WriteRawValue(JsonSerializer.SerializeToUtf8Bytes(value, knownType, GetOptionsCopy(options)));
            return;
        }

        throw new JsonException($"No serialisation support for {value.GetType()}");
    }
}


