using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Helpers;
using IvNav.Store.Identity.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Ardalis.GuardClauses;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

namespace IvNav.Store.Identity.Core.Configurations;

public static class RegisterCoreConfiguration
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructureDependencies(configuration);

        services.AddTransient<IUserManager, UserManager>();
        services.AddTransient<ISignInManager, SignInManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RegisterCoreConfiguration)));

        services.AddInteractionDependencies();

        return services;
    }

    public static AuthenticationBuilder AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var authSection = Guard.Against.Null(configuration.GetSection("AuthenticationSettings"));
        var identityServerSection = Guard.Against.Null(authSection.GetSection("IdentityServer"));

        var publicKey = Guard.Against.NullOrEmpty(identityServerSection.GetValue<string>("PublicKey"));
        var privateKey = Guard.Against.NullOrEmpty(identityServerSection.GetValue<string>("PrivateKey"));

        var cookieName = Guard.Against.NullOrEmpty(identityServerSection.GetValue<string>("CookieName"));

        var cleanupInterval = Guard.Against.Default(identityServerSection.GetValue<TimeSpan>("TokenCleanupInterval"));

        // Key
        var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        var key = new RsaSecurityKey(rsa);

        if (identityServerSection.GetValue<bool>("ShowPPI"))
        {
            IdentityModelEventSource.ShowPII = true;
        }

        services
            .AddIdentityServer(options =>
            {
                options.Authentication.CookieAuthenticationScheme = cookieName;
            })
            .AddSigningCredential(key, IdentityServerConstants.RsaSigningAlgorithm.PS256)
            .AddIdentityServerContext(configuration, cleanupInterval);

        services.AddAuthorization();

        return services
            .AddAuthentication(cookieName)
            .AddCookie(cookieName)
            .AddGoogle(GoogleDefaults.AuthenticationScheme, o =>
            {
                o.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                o.ClientId = authSection.GetValue<string>($"{GoogleDefaults.AuthenticationScheme}:ClientId")!;
                o.ClientSecret = authSection.GetValue<string>($"{GoogleDefaults.AuthenticationScheme}:ClientSecret")!;
            });
    }

    public static IApplicationBuilder UseCustomIdentityServer(this IApplicationBuilder app)
    {
        app.UseIdentityServer();

        return app;
    }
}
