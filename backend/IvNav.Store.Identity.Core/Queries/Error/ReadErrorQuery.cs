using Duende.IdentityServer.Services;
using MediatR;

namespace IvNav.Store.Identity.Core.Queries.Error;

internal class ReadErrorQuery : IRequestHandler<ReadErrorRequest, ReadErrorResponse>
{
    private readonly IIdentityServerInteractionService _identityServerInteractionService;

    public ReadErrorQuery(IIdentityServerInteractionService identityServerInteractionService)
    {
        _identityServerInteractionService = identityServerInteractionService;
    }


    public async Task<ReadErrorResponse> Handle(ReadErrorRequest request, CancellationToken cancellationToken)
    {
        var error = await _identityServerInteractionService.GetErrorContextAsync(request.ErrorId);

        return new ReadErrorResponse(error?.Error ?? "Unknown", error?.ErrorDescription);
    }
}
