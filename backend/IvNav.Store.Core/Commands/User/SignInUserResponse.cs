using System.Security.Claims;

namespace IvNav.Store.Core.Commands.User;

public class SignInUserResponse
{
    public static SignInUserResponse NotExists = new();
    public static SignInUserResponse InvalidPassword = new();
    public static SignInUserResponse EmailNotConfirmed = new();

    public IReadOnlyCollection<Claim>? Claims { get; }

    public bool Succeeded { get; }

    internal SignInUserResponse(IReadOnlyCollection<Claim>? claims)
    {
        Claims = claims;
        Succeeded = true;
    }

    private SignInUserResponse()
    {

    }
}
