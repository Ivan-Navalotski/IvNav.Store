using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

public class GrantConsentRequest : IRequest<GrantConsentResponse>
{
    public string? ReturnUrl { get; }

    public GrantConsentRequest(string? returnUrl)
    {
        ReturnUrl = returnUrl;
    }
}
