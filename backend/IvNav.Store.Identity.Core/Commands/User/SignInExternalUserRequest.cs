using System.Security.Claims;
using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

public class SignInExternalUserRequest : IRequest<SignInExternalUserResponse>
{
    public IReadOnlyCollection<Claim> Claims { get; }

    public string Provider { get; }

    public string? ReturnUrl { get; }

    public SignInExternalUserRequest(IReadOnlyCollection<Claim> claims, string provider, string? returnUrl)
    {
        Claims = Guard.Against.Null(claims);
        Provider = Guard.Against.NullOrEmpty(provider);
        ReturnUrl = returnUrl;
    }
}
