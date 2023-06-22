namespace IvNav.Store.Identity.Core.Commands.User;

public class GrantConsentResponse
{
    public bool Succeeded { get; }

    public GrantConsentResponse(bool succeeded)
    {
        Succeeded = succeeded;
    }
}
