using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class SignInUserCommand : IRequestHandler<SignInUserRequest, SignInUserResponse>
{
    private readonly IUserManager _userManager;

    public SignInUserCommand(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<SignInUserResponse> Handle(SignInUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userManager.SignIn(request.Email, request.Password, cancellationToken);

        return result.Succeeded
            ? new SignInUserResponse(await _userManager.GetClaims(result.UserId!.Value))
            : new SignInUserResponse(result.Errors);
    }
}
