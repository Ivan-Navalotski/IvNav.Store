using IvNav.Store.Common.Identity;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class SignOutUserCommand : IRequestHandler<SignOutUserRequest, SignOutUserResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;

    public SignOutUserCommand(IUserManager userManager, ISignInManager signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public async Task<SignOutUserResponse> Handle(SignOutUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetById(IdentityState.Current!.UserId, cancellationToken);

        await _signInManager.SignOut(user!, cancellationToken);

        return SignOutUserResponse.Success();
    }
}
