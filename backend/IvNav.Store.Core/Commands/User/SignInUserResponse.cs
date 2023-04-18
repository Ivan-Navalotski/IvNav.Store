namespace IvNav.Store.Core.Commands.User;

public class SignInUserResponse
{
    public static SignInUserResponse Error = new();

    public string? Token { get; }

    public SignInUserResponse(string? token)
    {
        Token = token;
    }

    private SignInUserResponse()
    {

    }
}
