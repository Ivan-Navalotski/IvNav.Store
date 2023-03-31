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
    /// UseLogger
    /// </summary>
    /// <param name="builder"></param>
    public static void UseLogger(this WebApplicationBuilder builder)
    {
        var section = builder.Configuration.GetSection("NLog");

        bool.TryParse(section.GetSection("clearProviders").Value, out var clearProviders);
        if (clearProviders)
        {
            builder.Logging.ClearProviders();
        }

        builder.Host.UseNLog();
    }
        
}
