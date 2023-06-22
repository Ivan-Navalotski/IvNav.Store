namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadIsInAuthorizationContextResponse
{
    public bool Value { get; }

    public ReadIsInAuthorizationContextResponse(bool value)
    {
        Value = value;
    }
}
