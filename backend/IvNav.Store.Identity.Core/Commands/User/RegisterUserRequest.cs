using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

public class RegisterUserRequest : IRequest<RegisterUserResponse>
{
    public string Email { get; }

    public string Password { get; }

    public string ConfirmationUrl { get; }

    public string? ConfirmationReturnUrl { get; }

    public RegisterUserRequest(string email, string password, string confirmationUrl, string? confirmationReturnUrl)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Password = Guard.Against.NullOrEmpty(password);
        ConfirmationUrl = Guard.Against.NullOrEmpty(confirmationUrl);
        ConfirmationReturnUrl = confirmationReturnUrl;
    }
}
