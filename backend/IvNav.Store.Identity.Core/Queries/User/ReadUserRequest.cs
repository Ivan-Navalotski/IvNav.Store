using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadUserRequest : IRequest<ReadUserResponse>
{
    public Guid Id { get; }

    public ReadUserRequest(Guid id)
    {
        Id = Guard.Against.Default(id);
    }
}
