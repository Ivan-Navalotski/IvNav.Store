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
        string? GetClaimValue(string type)
        {
            return request.Claims.FirstOrDefault(i => i.Type == type)?.Value;
        }

        var id = GetClaimValue(ClaimTypes.NameIdentifier);
        var email = GetClaimValue(ClaimTypes.Email);

        if (id == null || email == null) return SignInExternalUserResponse.InvalidClaims;


        var link = await _identityContext.UserExternalProviderLinks
            .Where(i => i.ExternalId == id && i.Provider == request.Provider)
            .FirstOrDefaultAsync(cancellationToken);

        if (link == null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) return SignInExternalUserResponse.Conflict;

            user = new Infrastructure.Entities.Identity.User
            {
                Email = email,
                UserName = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                NeedSetupPassword = true,

                GivenName = GetClaimValue(ClaimTypes.GivenName),
                Surname = GetClaimValue(ClaimTypes.Surname),
                DateOfBirth = DateTime.TryParse(GetClaimValue(ClaimTypes.DateOfBirth), out var dateOfBirth)
                    ? DateOnly.FromDateTime(dateOfBirth)
                    : null,
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
                    ExternalId = id,
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
