using MediatR;
using IvNav.Store.Identity.Core.Abstractions.Helpers;

namespace IvNav.Store.Identity.Core.Queries.User;

internal class ReadAuthClientQuery : IRequestHandler<ReadAuthClientRequest, ReadAuthClientResponse>
{
    private readonly ISignInManager _signInManager;

    public ReadAuthClientQuery(ISignInManager signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<ReadAuthClientResponse> Handle(ReadAuthClientRequest request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.GetClientInfoModel(request.ReturnUrl, cancellationToken);

        return new ReadAuthClientResponse(result);
    }
}
