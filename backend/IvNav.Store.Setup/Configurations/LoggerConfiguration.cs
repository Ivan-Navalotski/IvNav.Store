using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace IvNav.Store.Setup.Configurations;

/// <summary>
/// LoggerConfiguration
/// </summary>
public static class LoggerConfiguration
{
    /// <summary>
    /// Add logger
    /// </summary>
    /// <param name="builder"></param>
    public static void AddLogger(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("NLog");

        if (section.Value == null)
        {
            return;
        }

        if (bool.TryParse(section.GetSection("Logging:clearProviders").Value, out var clearProviders) && clearProviders)
        {
            builder.Logging.ClearProviders();
        }

        builder.Host.UseNLog();
    }
        
}
