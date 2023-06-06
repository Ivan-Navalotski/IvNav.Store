using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Core.Commands.User;

internal class RegisterUserCommand : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;

    public RegisterUserCommand(UserManager<Infrastructure.Entities.Identity.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            return RegisterUserResponse.EmailAlreadyExists;
        }

        var user = new Infrastructure.Entities.Identity.User
        {
            Email = request.Email,
            UserName = Guid.NewGuid().ToString(),
        };

        var userCreated = await _userManager.CreateAsync(user, request.Password);

        if (!userCreated.Succeeded)
        {
            return RegisterUserResponse.Error;
        }

        return new RegisterUserResponse(true);
    }
}
