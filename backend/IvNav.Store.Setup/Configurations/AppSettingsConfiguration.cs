using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// AppSettingsConfiguration
/// </summary>
public static class AppSettingsConfiguration
{
    /// <summary>
    /// UseAppSettings
    /// </summary>
    /// <param name="builder"></param>
    public static void UseAppSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings-api-info.json", true);
        builder.Configuration.AddJsonFile("appsettings-logger.json", true);
    }
}
