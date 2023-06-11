using IvNav.Store.Setup.Constants;
using Microsoft.AspNetCore.Authorization;

namespace IvNav.Store.Setup.Attributes;

public class OnlyForUserAuthorizeAttribute : AuthorizeAttribute
{
    public OnlyForUserAuthorizeAttribute() : base(policy: Policies.UserOnly)
    {

    }
}
