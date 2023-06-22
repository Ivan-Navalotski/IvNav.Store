using MediatR;

namespace IvNav.Store.Identity.Core.Queries.Error;

public class ReadErrorRequest : IRequest<ReadErrorResponse>
{
    public string? ErrorId { get; }
    public ReadErrorRequest(string? errorId)
    {
        ErrorId = errorId;
    }
}
