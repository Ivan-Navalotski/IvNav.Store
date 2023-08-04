using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Commands.User;

public class GrantConsentResponse
{
    public bool Succeeded { get; private init; }
    public string? ReturnUrl { get; private init; }

    private GrantConsentResponse()
    {

    }

    public static GrantConsentResponse Error()
    {
        return new GrantConsentResponse
        {
            Succeeded = false,
        };
    }

    public static GrantConsentResponse Success(string returnUrl)
    {
        return new GrantConsentResponse
        {
            Succeeded = true,
            ReturnUrl = Guard.Against.NullOrEmpty(returnUrl),
        };
    }
}
