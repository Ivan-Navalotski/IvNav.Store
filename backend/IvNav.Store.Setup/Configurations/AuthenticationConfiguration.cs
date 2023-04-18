using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenValidationParameters parameters)
    {
        services.AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = parameters;
        });

        return services;
    }
}
