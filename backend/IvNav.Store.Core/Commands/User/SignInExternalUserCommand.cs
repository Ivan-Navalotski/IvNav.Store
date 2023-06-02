using IvNav.Store.Core.Helpers;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Infrastructure.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IvNav.Store.Core.Commands.User;

internal class SignInExternalUserCommand : IRequestHandler<SignInExternalUserRequest, SignInExternalUserResponse>
{
    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;
    private readonly IIdentityContext _identityContext;

    public SignInExternalUserCommand(
        UserManager<Infrastructure.Entities.Identity.User> userManager,
        IIdentityContext identityContext)
    {
        _userManager = userManager;
        _identityContext = identityContext;
    }

    public async Task<SignInExternalUserResponse> Handle(SignInExternalUserRequest request, CancellationToken cancellationToken)
    {
        var idClaim = request.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier);
        var emailClaim = request.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Email);

        if (idClaim == null || emailClaim == null) return SignInExternalUserResponse.InvalidClaims;


        var link = await _identityContext.UserExternalProviderLinks
            .Where(i => i.ExternalId == idClaim.Value && i.Provider == request.Provider)
            .FirstOrDefaultAsync(cancellationToken);

        if (link == null)
        {
            var user = await _userManager.FindByEmailAsync(emailClaim.Value);
            if (user != null) return SignInExternalUserResponse.Conflict;

            user = new Infrastructure.Entities.Identity.User
            {
                Email = emailClaim.Value,
                UserName = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                NeedSetupPassword = true,
            };

            IdentityResult? userCreated = null;
            try
            {
                var password = $"{Guid.NewGuid()} {Guid.NewGuid().ToString().ToUpper()}!";
                userCreated = await _userManager.CreateAsync(user, password);

                if (!userCreated.Succeeded)
                {
                    return SignInExternalUserResponse.Error;
                }

                link = new UserExternalProviderLink
                {
                    Provider = request.Provider,
                    UserId = user.Id,
                    ExternalId = idClaim.Value,
                };

                await _identityContext.UserExternalProviderLinks.AddAsync(link, cancellationToken);
                await _identityContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                if (userCreated?.Succeeded ?? false)
                {
                    await _userManager.DeleteAsync(user);
                }

                return SignInExternalUserResponse.Error;
            }
        }

        var manager = new UserClaimManager();

        return new SignInExternalUserResponse(manager.GetUserClaims(link.UserId));
    }
}
