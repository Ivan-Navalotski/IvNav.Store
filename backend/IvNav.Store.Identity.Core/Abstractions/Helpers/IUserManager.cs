using System.Security.Claims;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Entities;

namespace IvNav.Store.Identity.Core.Abstractions.Helpers;

internal interface IUserManager
{
    Task<UserResultModel> CheckUserCredentials(string email, string password,
        CancellationToken cancellationToken);

    Task<UserResultModel> Create(string email, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null);

    Task<UserResultModel> CreateExternal(IReadOnlyCollection<Claim> claims, string provider,
        CancellationToken cancellationToken);

    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId,
        CancellationToken cancellationToken);

    Task<User?> GetById(Guid userId,
        CancellationToken cancellationToken);
}
