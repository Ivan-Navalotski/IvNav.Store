using System.Security.Claims;
using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Commands.User;

public class SignInExternalUserRequest : IRequest<SignInExternalUserResponse>
{
    public IReadOnlyCollection<Claim> Claims { get; }

    public string Provider { get; }

    public SignInExternalUserRequest(IReadOnlyCollection<Claim> claims, string provider)
    {
        Claims = Guard.Against.Null(claims);
        Provider = Guard.Against.NullOrEmpty(provider);
    }
}
