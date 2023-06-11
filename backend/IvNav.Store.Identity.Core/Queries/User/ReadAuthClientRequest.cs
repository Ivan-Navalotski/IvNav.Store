using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadAuthClientRequest : IRequest<ReadAuthClientResponse>
{
    public string? ReturnUrl { get; }

    public ReadAuthClientRequest(string? returnUrl)
    {
        ReturnUrl = returnUrl;
    }
}
