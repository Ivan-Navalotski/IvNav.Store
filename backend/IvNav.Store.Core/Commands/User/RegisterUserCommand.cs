using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Core.Commands.User;

internal class RegisterUserCommand : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;
    private readonly IInteractionClientManager _interactionClientManager;

    private static readonly string[] EmailErrors = {
        "InvalidEmail",
    };
    private static readonly string[] PasswordErrors = {
        "PasswordTooShort",
        "PasswordRequiresUniqueChars",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresLower",
        "PasswordRequiresUpper",
    };

    public RegisterUserCommand(UserManager<Infrastructure.Entities.Identity.User> userManager, IInteractionClientManager interactionClientManager)
    {
        _userManager = userManager;
        _interactionClientManager = interactionClientManager;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();

        var emailErrorKey = nameof(request.Email);
        var passwordErrorKey = nameof(request.Password);

        if (await _userManager.FindByEmailAsync(request.Email) != null)
        {
            AddError(errors, emailErrorKey, "DuplicateEmail");
            return new RegisterUserResponse(errors);
        }

        var user = new Infrastructure.Entities.Identity.User
        {
            Email = request.Email,
            UserName = Guid.NewGuid().ToString(),
        };

        var userCreated = await _userManager.CreateAsync(user, request.Password);

        if (!userCreated.Succeeded)
        {
            foreach (var userCreatedError in userCreated.Errors)
            {
                if (EmailErrors.Any(i => i == userCreatedError.Code))
                {
                    AddError(errors, emailErrorKey, userCreatedError.Code);
                }

                if (PasswordErrors.Any(i => i == userCreatedError.Code))
                {
                    AddError(errors, passwordErrorKey, userCreatedError.Code);
                }
            }

            return new RegisterUserResponse(errors);
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

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
            AddError(errors, emailErrorKey, "ErrorSendingConfirmationLink");
            await _userManager.DeleteAsync(user);
        }

        return new RegisterUserResponse(errors);
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
