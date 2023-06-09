using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Core.Commands.User;

internal class RegisterUserCommand : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;
    private readonly IInteractionClientManager _interactionClientManager;

    public RegisterUserCommand(UserManager<Infrastructure.Entities.Identity.User> userManager, IInteractionClientManager interactionClientManager)
    {
        _userManager = userManager;
        _interactionClientManager = interactionClientManager;
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

        try
        {
            var token = _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

            var body = $"{request.ConfirmationUrl}?token={token}";
            if (!string.IsNullOrEmpty(request.ConfirmationReturnUrl))
            {
                body += $"&returnUrl={request.ConfirmationReturnUrl}";
            }

            await _interactionClientManager.InvokeMethodAsync(
                HttpMethod.Post,
                AppId.MailService,
                "Emails",
                new
                {
                    To = request.Email,
                    Subject = "Please confirm email",
                    Body = body,
                },
                cancellationToken);
        }
        catch
        {
            await _userManager.DeleteAsync(user);

            throw;
        }

        return new RegisterUserResponse(true);
    }
}
