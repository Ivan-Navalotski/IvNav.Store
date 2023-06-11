using Duende.IdentityServer.Services;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Enums;
using IvNav.Store.Identity.Core.Extensions;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class SignInUserCommand : IRequestHandler<SignInUserRequest, SignInUserResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;

    public SignInUserCommand(IUserManager userManager, ISignInManager signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInUserResponse> Handle(SignInUserRequest request, CancellationToken cancellationToken)
    {
        var urlResult = await _signInManager.IsValidReturnUrl(request.ReturnUrl, cancellationToken);
        if (!urlResult.Succeeded)
        {
            return new SignInUserResponse(urlResult.Errors);
        }

        var result = await _userManager.CheckUserCredentials(request.Email, request.Password, cancellationToken);
        if (!result.Succeeded)
        {
            return new SignInUserResponse(result.Errors);
        }

        await _signInManager.SignIn(result.User!, cancellationToken);

        return new SignInUserResponse(urlResult.IsLocalUrl);
    }
}
