using IvNav.Store.Core.Extensions.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IvNav.Store.Core.Queries.User
{
    internal class ReadUserQuery : IRequestHandler<ReadUserRequest, ReadUserResponse>
    {
        private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;

        public ReadUserQuery(UserManager<Infrastructure.Entities.Identity.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ReadUserResponse> Handle(ReadUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            return user != null
                ? new ReadUserResponse(user.MapToModel())
                : ReadUserResponse.NotExists;
        }
    }
}
