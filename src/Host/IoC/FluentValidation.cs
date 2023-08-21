using FluentValidation;
using FluentValidation.AspNetCore;

namespace StorageSpike.Host.IoC;

internal static class FluentValidation
{
    internal static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();
    }
}
