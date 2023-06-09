using Ardalis.GuardClauses;
using IvNav.Store.Setup.Models;

namespace IvNav.Store.Setup.Attributes;

public class RequestHeadersAttribute : Attribute
{
    public RequestHeaderData[] Headers { get; protected set; }

    public RequestHeadersAttribute(params RequestHeaderData[] headers)
    {
        Headers = Guard.Against.Null(headers);
    }

    internal RequestHeadersAttribute()
    {
        Headers = Array.Empty<RequestHeaderData>();
    }
}
