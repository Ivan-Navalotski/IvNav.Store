using IvNav.Store.Core.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Core.Commands.User;

internal class SignInUserCommand : IRequestHandler<SignInUserRequest, SignInUserResponse>
{
    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;

    public SignInUserCommand(UserManager<Infrastructure.Entities.Identity.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SignInUserResponse> Handle(SignInUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return SignInUserResponse.NotExists;
        if (!await _userManager.CheckPasswordAsync(user, request.Password)) return SignInUserResponse.InvalidPassword;
        if (!await _userManager.IsEmailConfirmedAsync(user)) return SignInUserResponse.EmailNotConfirmed;

        var manager = new UserClaimManager();

        return new SignInUserResponse(manager.GetUserClaims(user.Id));
    }
}
