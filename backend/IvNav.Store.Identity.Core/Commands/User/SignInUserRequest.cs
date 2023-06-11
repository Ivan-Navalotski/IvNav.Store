using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInUserRequest : IRequest<SignInUserResponse>
{
    public string Email { get; }

    public string Password { get; }

    public bool RememberMe { get; }

    public string? ReturnUrl { get; }

    public SignInUserRequest(string email, string password, bool rememberMe, string? returnUrl)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Password = Guard.Against.NullOrEmpty(password);
        RememberMe = rememberMe;
        ReturnUrl = returnUrl;
    }
}
