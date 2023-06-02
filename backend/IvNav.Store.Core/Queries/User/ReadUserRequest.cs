using MediatR;

namespace IvNav.Store.Core.Queries.User;

public class ReadUserRequest : IRequest<ReadUserResponse>
{
    public string Email { get; set; }

    public string Provider { get; set; }
}
