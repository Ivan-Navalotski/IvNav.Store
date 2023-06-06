using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Core.Queries.User;

public class ReadUserRequest : IRequest<ReadUserResponse>
{
    public Guid Id { get; }

    public ReadUserRequest(Guid id)
    {
        Id = Guard.Against.Default(id);
    }
}
