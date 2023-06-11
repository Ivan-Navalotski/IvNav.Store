using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadIsValidReturnUrlRequest : IRequest<ReadIsValidReturnUrlResponse>
{
    public string? ReturnUrl { get; }

    public ReadIsValidReturnUrlRequest(string? returnUrl)
    {
        ReturnUrl = returnUrl;
    }
}
