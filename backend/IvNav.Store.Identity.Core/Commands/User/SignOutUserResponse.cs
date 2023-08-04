namespace IvNav.Store.Identity.Core.Commands.User;

public class SignOutUserResponse
{
    private SignOutUserResponse()
    {
    }

    internal static SignOutUserResponse Success()
    {
        return new SignOutUserResponse();
    }
}
