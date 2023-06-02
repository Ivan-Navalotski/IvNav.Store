using IvNav.Store.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace IvNav.Store.Web.Configurations;

/// <summary>
/// AuthenticationConfiguration
/// </summary>
public static class AuthenticationConfiguration
{
    /// <summary>
    /// Auth config
    /// </summary>
    public class AddJwtAuthenticationOptions
    {
        /// <summary>
        /// Validation params
        /// </summary>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new();

        internal AddJwtAuthenticationOptions()
        {
        }
    }

    /// <summary>
    /// AddAutoMapperProfiles
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddAuthentication(this IServiceCollection services,
        IConfiguration configuration, Action<AddJwtAuthenticationOptions>? options)
    {
        const string defScheme = "JWT_OR_COOKIE";

        services.AddAuthorization();

        var optionsData = new AddJwtAuthenticationOptions();
        options?.Invoke(optionsData);

        services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = defScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = optionsData.TokenValidationParameters;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Events.OnRedirectToLogin = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("");
                };

                o.Events.OnRedirectToAccessDenied = async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("");
                };
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
            {
                o.ClientId = configuration["Authentication:Google:ClientId"]!;
                o.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
            })
            .AddPolicyScheme(defScheme, "JWT_OR_COOKIE", o =>
            {
                // runs on each request
                o.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    var authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && ((string)authorization!).StartsWith("Bearer "))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }

                    // otherwise always check for cookie auth
                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

        return services;
    }
}
