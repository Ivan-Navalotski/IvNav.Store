using System.Security.Claims;

namespace IvNav.Store.Core.Commands.User;

public class SignInUserResponse
{
    public static SignInUserResponse Error = new();

    public IReadOnlyCollection<Claim>? Claims { get; }

    public SignInUserResponse(IReadOnlyCollection<Claim>? claims)
    {
        Claims = claims;
    }

    private SignInUserResponse()
    {

    }
}
