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
        var errors = new Dictionary<string, List<string>>();

        var emailErrorKey = nameof(request.Email);
        var passwordErrorKey = nameof(request.Password);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            AddError(errors, emailErrorKey, "UserNotExists");
            return new SignInUserResponse(errors);
        }
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            AddError(errors, emailErrorKey, "EmailNotConfirmed");
            return new SignInUserResponse(errors);
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            AddError(errors, passwordErrorKey, "IncorrectPassword");
        }

        var manager = new UserClaimManager();

        return new SignInUserResponse(manager.GetUserClaims(user.Id));
    }

    private static void AddError(IDictionary<string, List<string>> errors, string key, string error)
    {
        if (!errors.ContainsKey(key))
        {
            errors.Add(key, new List<string>());
        }

        errors[key].Add(error);
    }
}
