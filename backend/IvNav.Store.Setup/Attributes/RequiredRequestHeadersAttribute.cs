using IvNav.Store.Common.Constants;
using IvNav.Store.Setup.Models;

namespace IvNav.Store.Setup.Attributes;

public class RequiredRequestHeadersAttribute : RequestHeadersAttribute
{
    public RequiredRequestHeadersAttribute()
    {
        Headers = new RequestHeaderData[]
        {
            new(HeaderNames.UserId, "User identifer", isRequired: true, type: typeof(Guid)),
            new(HeaderNames.TenantId, "Tenant identifer", isRequired: false, type: typeof(Guid)),
        };
    }

    protected RequiredRequestHeadersAttribute(params RequestHeaderData[] headers) : base(headers)
    {

    }
}
