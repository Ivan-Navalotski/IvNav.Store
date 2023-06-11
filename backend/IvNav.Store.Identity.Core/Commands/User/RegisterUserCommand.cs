using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Enums;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Enums;
using IvNav.Store.Identity.Core.Extensions;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class RegisterUserCommand : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IUserManager _userManager;
    private readonly IInteractionClientManager _interactionClientManager;


    public RegisterUserCommand(IUserManager userManager, IInteractionClientManager interactionClientManager)
    {
        _userManager = userManager;
        _interactionClientManager = interactionClientManager;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userManager.Create(request.Email, request.Password, cancellationToken,
            async (userId, errors) =>
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userId, cancellationToken);

                var body = $"{request.ConfirmationUrl}?token={token}";
                if (!string.IsNullOrEmpty(request.ConfirmationReturnUrl))
                {
                    body += $"&returnUrl={request.ConfirmationReturnUrl}";
                }

                try
                {
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
                    errors.AddUserError(UserErrors.EmailError.ConfirmationLinkSendingError);
                }
            });

        return result.Succeeded
            ? new RegisterUserResponse()
            : new RegisterUserResponse(result.Errors);
    }
}
