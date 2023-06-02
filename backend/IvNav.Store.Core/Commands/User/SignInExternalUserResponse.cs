using System.Security.Claims;

namespace IvNav.Store.Core.Commands.User;

public class SignInExternalUserResponse
{
    public static SignInExternalUserResponse InvalidProvider = new();
    public static SignInExternalUserResponse InvalidClaims = new();
    public static SignInExternalUserResponse Error = new();
    public static SignInExternalUserResponse Conflict = new();

    public IReadOnlyCollection<Claim>? Claims { get; }

    public bool Succeeded { get; }

    public SignInExternalUserResponse(IReadOnlyCollection<Claim>? claims)
    {
        Claims = claims;
        Succeeded = true;
    }

    private SignInExternalUserResponse()
    {

    }
}
