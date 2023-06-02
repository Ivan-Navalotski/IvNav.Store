namespace IvNav.Store.Core.Commands.User;

public class RegisterUserResponse
{
    public static RegisterUserResponse EmailAlreadyExists = new();

    public static RegisterUserResponse Error = new();

    public bool Succeeded { get; }

    public RegisterUserResponse(bool succeeded)
    {
        Succeeded = succeeded;
    }

    private RegisterUserResponse()
    {

    }
}
