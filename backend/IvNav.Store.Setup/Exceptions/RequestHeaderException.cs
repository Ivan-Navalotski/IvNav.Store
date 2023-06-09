using IvNav.Store.Setup.Models;

namespace IvNav.Store.Setup.Exceptions;

internal class RequestHeaderException : InvalidOperationException
{
    public RequestHeaderData RequestHeader { get; }

    public RequestHeaderException(RequestHeaderData requestHeader, string message) : base(message)
    {
        RequestHeader = requestHeader;
    }
}
