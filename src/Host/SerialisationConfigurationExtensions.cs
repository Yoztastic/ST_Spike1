using Host.Contracts.Serialisation;
using Microsoft.AspNetCore.Mvc;

namespace StorageSpike.Host;

public static class SerialisationConfigurationExtensions
{
    public static void Configure(this JsonOptions jsonOptions)
    {
        foreach (var optionsConverter in Serialisation.Options.Converters)
            jsonOptions.JsonSerializerOptions.Converters.Add(optionsConverter);

        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = Serialisation.Options.DefaultIgnoreCondition;
        jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive =
            Serialisation.Options.PropertyNameCaseInsensitive;
    }
}