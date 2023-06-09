using IvNav.Store.Core.Interaction.Enums;

namespace IvNav.Store.Core.Interaction.Abstractions.Helpers;

public interface IInteractionClientManager
{
    Task InvokeMethodAsync(HttpMethod method, AppId appId, string methodName,
        CancellationToken cancellationToken);

    Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod method, AppId appId, string methodName,
        CancellationToken cancellationToken);

    Task InvokeMethodAsync<TRequest>(HttpMethod method, AppId appId, string methodName,
        TRequest data,
        CancellationToken cancellationToken);

    Task<TResponse> InvokeMethodAsync<TRequest, TResponse>(HttpMethod method, AppId appId,
        string methodName,
        TRequest data,
        CancellationToken cancellationToken);
}
