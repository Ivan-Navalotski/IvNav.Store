using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// AppSettingsConfiguration
/// </summary>
public static class AppSettingsConfiguration
{
    /// <summary>
    /// AddAppSettings
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="fileNames"></param>
    public static void AddAppSettings(this WebApplicationBuilder builder, params string[] fileNames)
    {
        foreach (var fileName in fileNames)
        {
            builder.Configuration.AddJsonFile(fileName, true);
        }
    }
}
