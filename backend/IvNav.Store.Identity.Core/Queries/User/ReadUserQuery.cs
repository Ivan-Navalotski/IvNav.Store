using IvNav.Store.Identity.Core.Abstractions.Helpers;
using MediatR;

namespace IvNav.Store.Identity.Core.Queries.User
{
    internal class ReadUserQuery : IRequestHandler<ReadUserRequest, ReadUserResponse>
    {
        private readonly IUserManager _userManager;

        public ReadUserQuery(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<ReadUserResponse> Handle(ReadUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetById(request.Id, cancellationToken);

            return user != null
                ? new ReadUserResponse(user)
                : ReadUserResponse.NotExists;
        }
    }
}
