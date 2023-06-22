using System.Security.Claims;
using IvNav.Store.Identity.Core.Abstractions.Helpers;
using IvNav.Store.Identity.Core.Enums;
using IvNav.Store.Identity.Core.Extensions;
using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Abstractions.Contexts;
using IvNav.Store.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Identity.Core.Helpers;

internal class UserManager : IUserManager
{
    private readonly UserManager<User> _userManager;
    private readonly IAppDbContext _appDbContext;

    public UserManager(UserManager<User> userManager, IAppDbContext appDbContext)
    {
        _userManager = userManager;
        _appDbContext = appDbContext;
    }

    public async Task<UserResultModel> CheckUserCredentials(string email, string password,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            errors.AddUserError(UserErrors.EmailError.UserNotExists);
            return new UserResultModel(errors);
        }
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            errors.AddUserError(UserErrors.EmailError.EmailNotConfirmed);
            return new UserResultModel(errors);
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            errors.AddUserError(UserErrors.PasswordError.IncorrectPassword);
            return new UserResultModel(errors);
        }

        return new UserResultModel(user);
    }

    public Task<UserResultModel> Create(string email, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null)
    {
        var user = new User
        {
            Email = email,
            UserName = email,
        };

        return Create(user, password, Array.Empty<Claim>(), cancellationToken, onAfterCreated);
    }

    private async Task<UserResultModel> Create(User user, string password,
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null)
    {
        var errors = new Dictionary<string, List<string>>();

        if (await _userManager.FindByEmailAsync(user.Email!) != null)
        {
            errors.AddUserError(UserErrors.EmailError.DuplicateEmail);
            return new UserResultModel(errors);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var userCreated = await _userManager.CreateAsync(user, password);

        if (!userCreated.Succeeded)
        {
            foreach (var userCreatedError in userCreated.Errors)
            {
                if (Enum.GetValues<UserErrors.EmailError>().Any(i => i.ToString() == userCreatedError.Code))
                {
                    errors.AddUserError(Enum.Parse<UserErrors.EmailError>(userCreatedError.Code));
                }

                if (Enum.GetValues<UserErrors.PasswordError>().Any(i => i.ToString() == userCreatedError.Code))
                {
                    errors.AddUserError(Enum.Parse<UserErrors.PasswordError>(userCreatedError.Code));
                }
            }

            return new UserResultModel(errors);
        }

        try
        {
            // Claims
            var claimsToAdd = claims
                .Where(i => i.Type != ClaimTypes.NameIdentifier)
                .ToList();
            claimsToAdd.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            await _userManager.AddClaimsAsync(user, claimsToAdd);

            // AfterCreated invoke
            if (onAfterCreated != null)
            {
                await onAfterCreated.Invoke(user.Id, errors);
            }

            if (errors.Any())
            {
                await _userManager.DeleteAsync(user);
            }
        }
        catch
        {
            await _userManager.DeleteAsync(user);
            return new UserResultModel(errors);
        }

        return !errors.Any()
            ? new UserResultModel(user)
            : new UserResultModel(errors);
    }

    public async Task<UserResultModel> CreateExternal(IReadOnlyCollection<Claim> claims, string provider,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();

        var idExternal = GetClaimValue(claims, ClaimTypes.NameIdentifier);
        var email = GetClaimValue(claims, ClaimTypes.Email);

        if (idExternal == null)
        {
            errors.AddUserError(UserErrors.ExternalIdError.InvalidExternalId);
        }
        if (email == null)
        {
            errors.AddUserError(UserErrors.EmailError.InvalidEmail);
        }

        if (errors.Any())
        {
            return new UserResultModel(errors);
        }

        var link = await _appDbContext.UserExternalProviderLinks
            .Where(i => i.ExternalId == idExternal && i.Provider == provider)
            .FirstOrDefaultAsync(cancellationToken);

        User user;

        if (link == null)
        {
            var userFromManager = await _userManager.FindByEmailAsync(email!);
            if (userFromManager != null)
            {
                errors.AddUserError(UserErrors.EmailError.UserAlreadyExists);
                return new UserResultModel(errors);
            }

            var claimTypes = new List<string>
            {
                ClaimTypes.Email,
                ClaimTypes.GivenName,
                ClaimTypes.Surname,
                ClaimTypes.DateOfBirth,
            };

            var claimsForUser = claims
                .Where(i => claimTypes.Contains(i.Type))
                .ToArray();

            user = new User
            {
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                NeedSetupPassword = true,
            };

            var password = $"{Guid.NewGuid()} {Guid.NewGuid().ToString().ToUpper()}!";

            var userCreated = await Create(user, password, claimsForUser, cancellationToken);

            if (!userCreated.Succeeded)
            {
                return userCreated;
            }

            try
            {
                link = new UserExternalProviderLink
                {
                    Provider = provider,
                    UserId = user.Id,
                    ExternalId = idExternal!,
                };

                await _appDbContext.UserExternalProviderLinks.AddAsync(link, cancellationToken);
                await _appDbContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _userManager.DeleteAsync(user);

                errors.AddUserError(UserErrors.EmailError.ErrorCreatingExternalLink);

                return new UserResultModel(errors);
            }
        }
        else
        {
            user = (await _userManager.FindByIdAsync(link.UserId.ToString()))!;
        }

        return new UserResultModel(user);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return await _userManager.GenerateEmailConfirmationTokenAsync(user!);
    }

    public async Task<User?> GetById(Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user;
    }

    private static string? GetClaimValue(IEnumerable<Claim> claims, string type)
    {
        return claims.FirstOrDefault(i => i.Type == type)?.Value;
    }
}
