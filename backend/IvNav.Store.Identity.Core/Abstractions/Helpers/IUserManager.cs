using System.Security.Claims;
using IvNav.Store.Identity.Core.Models.User;

namespace IvNav.Store.Identity.Core.Abstractions.Helpers;

internal interface IUserManager
{
    IReadOnlyList<Claim> GetClaims(Guid userId);

    Task<UserManagerReultModel> Create(string email, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null);

    Task<UserManagerReultModel> CreateExternal(IReadOnlyCollection<Claim> claims, string provider,
        CancellationToken cancellationToken);

    Task<UserManagerReultModel> SignIn(string email, string password,
        CancellationToken cancellationToken);

    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId,
        CancellationToken cancellationToken);

    Task<UserModel?> GetById(Guid userId,
        CancellationToken cancellationToken);
}
