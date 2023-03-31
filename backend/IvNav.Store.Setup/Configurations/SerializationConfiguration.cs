using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// SerializationConfiguration
/// </summary>
public static class SerializationConfiguration
{
    /// <summary>
    /// RegisterJsonOptions
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterJsonOptions(this IServiceCollection services)
    {
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNameCaseInsensitive = false;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
        });

        return services;
    }
}
