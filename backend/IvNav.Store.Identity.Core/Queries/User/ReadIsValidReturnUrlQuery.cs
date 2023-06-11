using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

internal class ReadIsValidReturnUrlQuery : IRequestHandler<ReadIsValidReturnUrlRequest, ReadIsValidReturnUrlResponse>
{
    private readonly ISignInManager _signInManager;

    public ReadIsValidReturnUrlQuery(ISignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ReadIsValidReturnUrlResponse> Handle(ReadIsValidReturnUrlRequest request, CancellationToken cancellationToken)
    {
        var urlResult = await _signInManager.IsValidReturnUrl(request.ReturnUrl, cancellationToken);
        if (!urlResult.Succeeded)
        {
            return new ReadIsValidReturnUrlResponse(urlResult.Errors, urlResult.IsLocalUrl);
        }

        return new ReadIsValidReturnUrlResponse(urlResult.IsLocalUrl);
    }
}
