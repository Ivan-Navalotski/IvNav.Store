using MediatR;
using IvNav.Store.Core.Abstractions.Helpers;

namespace IvNav.Store.Core.Commands.User;

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
