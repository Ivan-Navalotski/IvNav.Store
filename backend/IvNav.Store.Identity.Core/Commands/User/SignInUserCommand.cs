using IvNav.Store.Identity.Core.Abstractions.Helpers;
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
        var result = await _userManager.CheckUserCredentials(request.Email, request.Password, cancellationToken);
        if (!result.Succeeded)
        {
            return SignInUserResponse.Error(result.Errors);
        }

        await _signInManager.SignIn(result.User!, cancellationToken);

        return SignInUserResponse.Success(result.User!.Id);
    }
}
