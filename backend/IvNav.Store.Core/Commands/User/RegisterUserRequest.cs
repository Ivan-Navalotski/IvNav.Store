using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Commands.User;

public class RegisterUserRequest : IRequest<RegisterUserResponse>
{
    public string Email { get; }

    public string Password { get; }

    public RegisterUserRequest(string email, string password)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Password = Guard.Against.NullOrEmpty(password);
    }
}
