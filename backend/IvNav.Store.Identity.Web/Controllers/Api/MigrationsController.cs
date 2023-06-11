using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IvNav.Store.Setup.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace IvNav.Store.Identity.Web.Controllers.Api
{
    public class MigrationsController : ApiControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationsController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RecreateIdentityEntities()
        {
            var context = _serviceProvider.GetRequiredService<ConfigurationDbContext>();

            context.Clients.RemoveRange(context.Clients.ToList());
            context.IdentityResources.RemoveRange(context.IdentityResources.ToList());
            context.ApiResources.RemoveRange(context.ApiResources.ToList());
            context.ApiScopes.RemoveRange(context.ApiScopes.ToList());
            await context.SaveChangesAsync();

            if (!context.Clients.Any())
            {
                foreach (var client in Config.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.GetApiScopes())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                await context.SaveChangesAsync();
            }

            return NoContent();
        }
    }

    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(name: "WebApi", displayName: "MyAPI", new []
                {
                    ClaimTypes.NameIdentifier,
                })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "WebApi",
                    Scopes = new List<string> { "Identity" },
                    UserClaims =
                    {
                        //Custom user claims that should be provided when requesting access to this API.
                        //These claims will be added to the access token, not the ID token!
                        ClaimTypes.NameIdentifier,
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "WebApi" },

                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                }
            };
        }
    }
}
