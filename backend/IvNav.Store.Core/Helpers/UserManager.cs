using IvNav.Store.Common.Constants;
using IvNav.Store.Core.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IvNav.Store.Core.Extensions.Common;
using IvNav.Store.Core.Abstractions.Helpers;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IvNav.Store.Core.Helpers;

internal class UserManager : IUserManager
{
    private static readonly string EmailErrorKey = "Email";
    private static readonly string PasswordErrorKey = "Password";

    private static readonly string[] EmailErrors = {
        "InvalidEmail",
    };
    private static readonly string[] PasswordErrors = {
        "PasswordTooShort",
        "PasswordRequiresUniqueChars",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresNonAlphanumeric",
        "PasswordRequiresLower",
        "PasswordRequiresUpper",
    };

    private readonly UserManager<Infrastructure.Entities.Identity.User> _userManager;
    private readonly IIdentityContext _context;

    public UserManager(UserManager<Infrastructure.Entities.Identity.User> userManager, IIdentityContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IReadOnlyList<Claim> GetClaims(Guid userId)
    {
        var claims = new[]
        {
            new Claim(ClaimIdentityConstants.UserIdClaimType, userId.ToString()),
            new Claim(ClaimIdentityConstants.TenantIdClaimType, Guid.NewGuid().ToString()),
        };

        return claims;
    }

    public Task<UserManagerReultModel> Create(string email, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null)
    {
        var user = new Infrastructure.Entities.Identity.User
        {
            Email = email,
            UserName = Guid.NewGuid().ToString(),
        };

        return Create(user, password, cancellationToken, onAfterCreated);
    }

    private async Task<UserManagerReultModel> Create(Infrastructure.Entities.Identity.User user, string password,
        CancellationToken cancellationToken,
        Func<Guid, Dictionary<string, List<string>>, Task>? onAfterCreated = null)
    {
        var errors = new Dictionary<string, List<string>>();

        if (await _userManager.FindByEmailAsync(user.Email!) != null)
        {
            errors.Upsert(EmailErrorKey, "DuplicateEmail");
            return new UserManagerReultModel(errors);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var userCreated = await _userManager.CreateAsync(user, password);

        if (!userCreated.Succeeded)
        {
            foreach (var userCreatedError in userCreated.Errors)
            {
                if (EmailErrors.Any(i => i == userCreatedError.Code))
                {
                    errors.Upsert(EmailErrorKey, userCreatedError.Code);
                }

                if (PasswordErrors.Any(i => i == userCreatedError.Code))
                {
                    errors.Upsert(PasswordErrorKey, userCreatedError.Code);
                }
            }

            return new UserManagerReultModel(errors);
        }

        try
        {
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
            return new UserManagerReultModel(errors);
        }

        return !errors.Any()
            ? new UserManagerReultModel(user.Id)
            : new UserManagerReultModel(errors);
    }

    public async Task<UserManagerReultModel> CreateExternal(IReadOnlyCollection<Claim> claims, string provider,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();

        var id = GetClaimValue(claims, ClaimTypes.NameIdentifier);
        var email = GetClaimValue(claims, ClaimTypes.Email);

        if (id == null)
        {
            errors.Upsert("Id", "ValueIsMissing");
        }
        if (email == null)
        {
            errors.Upsert(EmailErrorKey, "ValueIsMissing");
        }

        if (errors.Any())
        {
            return new UserManagerReultModel(errors);
        }

        var link = await _context.UserExternalProviderLinks
            .Where(i => i.ExternalId == id && i.Provider == provider)
            .FirstOrDefaultAsync(cancellationToken);

        var userId = link?.UserId;

        if (link == null)
        {
            var user = await _userManager.FindByEmailAsync(email!);
            if (user != null)
            {
                errors.Upsert(EmailErrorKey, "UserAlreadyExists");
                return new UserManagerReultModel(errors);
            }

            user = new Infrastructure.Entities.Identity.User
            {
                Email = email,
                UserName = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                NeedSetupPassword = true,

                GivenName = GetClaimValue(claims, ClaimTypes.GivenName),
                Surname = GetClaimValue(claims, ClaimTypes.Surname),
                DateOfBirth = DateTime.TryParse(GetClaimValue(claims, ClaimTypes.DateOfBirth), out var dateOfBirth)
                    ? DateOnly.FromDateTime(dateOfBirth)
                    : null,
            };

            var password = $"{Guid.NewGuid()} {Guid.NewGuid().ToString().ToUpper()}!";

            var userCreated = await Create(user, password, cancellationToken);

            if (!userCreated.Succeeded)
            {
                return userCreated;
            }

            userId = user.Id;

            try
            {
                link = new Infrastructure.Entities.Identity.UserExternalProviderLink
                {
                    Provider = provider,
                    UserId = user.Id,
                    ExternalId = id!,
                };

                await _context.UserExternalProviderLinks.AddAsync(link, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _userManager.DeleteAsync(user);

                errors.Upsert(EmailErrorKey, "ErrorCreatingExternalLink");

                return new UserManagerReultModel(errors);
            }
        }

        return new UserManagerReultModel(userId!.Value);
    }

    public Task Delete(Infrastructure.Entities.Identity.User user,
        CancellationToken cancellationToken)
    {
        return _userManager.DeleteAsync(user);
    }

    public async Task<UserManagerReultModel> SignIn(string email, string password,
        CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            errors.Upsert(EmailErrorKey, "UserNotExists");
            return new UserManagerReultModel(errors);
        }
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            errors.Upsert(EmailErrorKey, "EmailNotConfirmed");
            return new UserManagerReultModel(errors);
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            errors.Upsert(PasswordErrorKey, "IncorrectPassword");
            return new UserManagerReultModel(errors);
        }

        return new UserManagerReultModel(user.Id);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return await _userManager.GenerateEmailConfirmationTokenAsync(user!);
    }

    private static string? GetClaimValue(IEnumerable<Claim> claims, string type)
    {
        return claims.FirstOrDefault(i => i.Type == type)?.Value;
    }
}
