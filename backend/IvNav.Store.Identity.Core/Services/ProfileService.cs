using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using IvNav.Store.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Identity.Core.Services
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // add actor claim if needed
            if (context.Subject.GetAuthenticationMethod() == OidcConstants.GrantTypes.TokenExchange)
            {
                var act = context.Subject.FindFirst(JwtClaimTypes.Actor);
                if (act != null)
                {
                    context.IssuedClaims.Add(act);
                }
            }

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
