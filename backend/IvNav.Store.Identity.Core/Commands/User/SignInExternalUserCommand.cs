using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class SignInExternalUserCommand : IRequestHandler<SignInExternalUserRequest, SignInExternalUserResponse>
{
    private readonly IUserManager _userManager;

    public SignInExternalUserCommand(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<SignInExternalUserResponse> Handle(SignInExternalUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateExternal(request.Claims, request.Provider, cancellationToken);

        return result.Succeeded
            ? new SignInExternalUserResponse(_userManager.GetClaims(result.UserId!.Value))
            : new SignInExternalUserResponse(result.Errors);
    }
}
