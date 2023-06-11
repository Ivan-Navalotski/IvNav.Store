using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Setup.Configurations;

public static class AuthenticationConfiguration
{
    public static AuthenticationBuilder AddJwtBearerFromConfig(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        var authSection = Guard.Against.Null(configuration.GetSection("AuthenticationSettings:JWT"));

        return builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
        {
            o.SaveToken = true;

            o.Authority = Guard.Against.Null(authSection.GetValue<string>("Authority"));
            o.Audience = Guard.Against.Null(authSection.GetValue<string>("Audience"));
        });
    }
}
