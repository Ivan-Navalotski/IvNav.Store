using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Commands.User;

public class SignInUserRequest : IRequest<SignInUserResponse>
{
    public string Email { get; }

    public string Password { get; }

    public SignInUserRequest(string email, string password)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Password = Guard.Against.NullOrEmpty(password);
    }
}
