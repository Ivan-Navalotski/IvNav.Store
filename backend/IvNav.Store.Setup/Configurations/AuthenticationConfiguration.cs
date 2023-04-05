using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// AuthenticationConfiguration
/// </summary>
public static class AuthenticationConfiguration
{
    /// <summary>
    /// AddAutoMapperProfiles
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthorization();

        return services;
    }
}
