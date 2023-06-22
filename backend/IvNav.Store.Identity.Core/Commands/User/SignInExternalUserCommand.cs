using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class SignInExternalUserCommand : IRequestHandler<SignInExternalUserRequest, SignInExternalUserResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;
    

    public SignInExternalUserCommand(IUserManager userManager, ISignInManager signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInExternalUserResponse> Handle(SignInExternalUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateExternal(request.Claims, request.Provider, cancellationToken);

        if (!result.Succeeded)
        {
            return new SignInExternalUserResponse(result.Errors);
        }

        await _signInManager.SignIn(result.User!, cancellationToken);

        return new SignInExternalUserResponse();
    }
}
