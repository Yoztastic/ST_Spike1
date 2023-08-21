using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StorageSpike.Host.IoC;

internal static class Swagger
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "StorageSpike", Version = "v1" });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    PlatformServices.Default.Application.ApplicationName + ".xml");

                if (File.Exists(filePath)) options.IncludeXmlComments(filePath);
                options.OperationFilter<RequiredHeadersSwaggerOperationFilter>();
            })
            .AddFluentValidationRulesToSwagger();


    private sealed class RequiredHeadersSwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>(3);

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = DefaultHeaders.ConversationId,
                In = ParameterLocation.Header,
                Required = true,
                Example = new OpenApiString("SwaggerConversationId")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = DefaultHeaders.ContextIdentity,
                In = ParameterLocation.Header,
                Required = false,
                Example = new OpenApiString("simon.pettman@uniper.com")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = DefaultHeaders.UserAgent,
                In = ParameterLocation.Header,
                Required = true,
                Example = new OpenApiString("Swagger/1.2.3")
            });
        }
    }
}
