using IvNav.Store.Core.Models.User;
using System.Security.Claims;

namespace IvNav.Store.Core.Abstractions.Helpers;

internal interface IUserManager
{
    IReadOnlyList<Claim> GetClaims(Guid userId);

    Task<UserManagerReultModel> Create(string email, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null);

    Task<UserManagerReultModel> CreateExternal(IReadOnlyCollection<Claim> claims, string provider,
        CancellationToken cancellationToken);

    Task Delete(Infrastructure.Entities.Identity.User user,
        CancellationToken cancellationToken);

    Task<UserManagerReultModel> SignIn(string email, string password,
        CancellationToken cancellationToken);

    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId,
        CancellationToken cancellationToken);
}
