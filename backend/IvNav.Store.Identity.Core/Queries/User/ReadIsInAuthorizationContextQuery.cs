using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

internal class ReadIsInAuthorizationContextQuery : IRequestHandler<ReadIsInAuthorizationContextRequest, ReadIsInAuthorizationContextResponse>
{
    private readonly ISignInManager _signInManager;

    public ReadIsInAuthorizationContextQuery(ISignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ReadIsInAuthorizationContextResponse> Handle(ReadIsInAuthorizationContextRequest request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.IsInAuthorizationContext(request.ReturnUrl, cancellationToken);
        return new ReadIsInAuthorizationContextResponse(result);
    }
}
