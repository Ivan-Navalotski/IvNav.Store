using Ardalis.GuardClauses;
using Duende.IdentityServer;
using Google.Api;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IvNav.Store.Identity.Web.Configurations;

/// <summary>
/// AuthenticationConfiguration
/// </summary>
public static class AuthenticationConfiguration
{
    /// <summary>
    /// AddAutoMapperProfiles
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthorization();

        var authSection = Guard.Against.Null(configuration.GetSection("AuthenticationSettings"));

        services
            .AddAuthentication()
            //.AddCookie("MyCookie")
            //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            //{
            //    o.Authority = authSection.GetValue<string>("IdentityServer:Authority");
            //    o.Audience = "Identity";
            //    o.RequireHttpsMetadata = false;
            //    o.SaveToken = true;
            //})
            //.AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
            //{
            //    o.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    o.ClientId = authSection.GetValue<string>("Google:ClientId")!;
            //    o.ClientSecret = authSection.GetValue<string>("Google:ClientSecret")!;
            //});
            ;

        return services;
    }
}
