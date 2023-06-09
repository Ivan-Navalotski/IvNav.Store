using System.Net;
using IvNav.Store.Core.Interaction.Enums;

namespace IvNav.Store.Core.Interaction.Exceptions;

internal class InvocationException : Exception
{
    public AppId AppId { get; }
    public HttpStatusCode StatusCode { get; }



    public InvocationException(AppId appId, HttpStatusCode statusCode, string message) : base(message)
    {
        AppId = appId;
        StatusCode = statusCode;
    }

    public InvocationException(AppId appId, HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
    {
        AppId = appId;
        StatusCode = statusCode;
    }
}
