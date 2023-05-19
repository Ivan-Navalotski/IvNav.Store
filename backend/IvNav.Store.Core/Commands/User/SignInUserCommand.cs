using System.Security.Claims;
using IvNav.Store.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace IvNav.Store.Core.Commands.User;

internal class SignInUserCommand : IRequestHandler<SignInUserRequest, SignInUserResponse>
{
    private readonly IConfiguration _configuration;
    UserManager<Infrastructure.Entities.Identity.User> _userManager;

    public SignInUserCommand(IConfiguration configuration, UserManager<Infrastructure.Entities.Identity.User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<SignInUserResponse> Handle(SignInUserRequest request, CancellationToken cancellationToken)
    {
        var badResponse = SignInUserResponse.Error;

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return badResponse;
        if (! await _userManager.CheckPasswordAsync(user, request.Password)) return badResponse;

        var claims = new[]
        {
            new Claim(ClaimIdentityConstants.UserIdClaimType, user.Id),
            new Claim(ClaimIdentityConstants.TenantIdClaimType, Guid.NewGuid().ToString()),
        };

        return new SignInUserResponse(claims);
    }
}
