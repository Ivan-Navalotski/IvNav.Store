using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadIsInAuthorizationContextRequest : IRequest<ReadIsInAuthorizationContextResponse>
{
    public string? ReturnUrl { get; }

    public ReadIsInAuthorizationContextRequest(string? returnUrl)
    {
        ReturnUrl = returnUrl;
    }
}
