using IvNav.Store.Identity.Core.Models.User;
using IvNav.Store.Identity.Infrastructure.Entities;

namespace IvNav.Store.Identity.Core.Abstractions.Helpers;

internal interface ISignInManager
{
    Task<ClientInfoModel?> GetClientInfoModel(string? returnUrl, CancellationToken cancellationToken);

    Task<ValidateReturnUrlResultModel> IsValidReturnUrl(string? returnUrl, CancellationToken cancellationToken);

    Task<UserResultModel> SignIn(User user, CancellationToken cancellationToken);

    Task SignOut(User user, CancellationToken cancellationToken);
}
