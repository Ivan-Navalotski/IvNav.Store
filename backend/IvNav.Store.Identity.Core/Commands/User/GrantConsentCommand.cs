using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Commands.User;

internal class GrantConsentCommand : IRequestHandler<GrantConsentRequest, GrantConsentResponse>
{
    private readonly ISignInManager _signInManager;

    public GrantConsentCommand(ISignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<GrantConsentResponse> Handle(GrantConsentRequest request, CancellationToken cancellationToken)
    {
        if (!await _signInManager.IsInAuthorizationContext(request.ReturnUrl, cancellationToken))
        {
            return GrantConsentResponse.Error();
        }
        var result = await _signInManager.GrantConsent(request.ReturnUrl, cancellationToken);

        return GrantConsentResponse.Success(result!);
    }
}
