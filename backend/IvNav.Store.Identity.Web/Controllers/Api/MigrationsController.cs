using System.Security.Claims;
using Duende.IdentityServer;
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
                new IdentityResources.Profile
                {
                    UserClaims = new List<string>
                    {
                        ClaimTypes.NameIdentifier,
                    }
                },
                new IdentityResources.OpenId
                {
                    UserClaims = new List<string>
                    {
                        ClaimTypes.NameIdentifier,
                    }
                },
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "Identity",
                    Scopes = new List<string>
                    {
                        "WebApi",
                    }
                },
                new ApiResource
                {
                    Name = "WebApi",
                    Scopes = new List<string>
                    {
                        "WebApi",
                    }
                },
                new ApiResource
                {
                    Name = "Product",
                    Scopes = new List<string>
                    {
                        "WebApi",
                    }
                },
                new ApiResource
                {
                    Name = "Mail",
                    Scopes = new List<string>
                    {
                        "WebApi",
                    }
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new("WebApi", new List<string> { ClaimTypes.NameIdentifier }),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new()
                {
                    ClientId = "WebApiClient",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("WebApiSecret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        "WebApi",
                    },

                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                },
                new()
                {
                    ClientId = "PortalClient",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("PortalClientSecret".Sha256())
                    },
                    RequireClientSecret = false,

                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "WebApi",
                    },

                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200"
                    },
                    RequireConsent = false,
                    AccessTokenLifetime = 600
                },
            };
        }
    }
}
