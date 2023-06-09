using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Dapr.Client;
using IvNav.Store.Common.Constants;
using IvNav.Store.Common.Extensions;
using IvNav.Store.Common.Identity;
using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Core.Interaction.Enums;
using IvNav.Store.Core.Interaction.Extensions;
using Microsoft.Extensions.Logging;

namespace IvNav.Store.Core.Interaction.Helpers
{
    internal class InteractionClientManager : IInteractionClientManager
    {
        private readonly ILogger<InteractionClientManager> _logger;
        private readonly DaprClient _daprClient;

        private readonly string _apiPrefix;

        public InteractionClientManager(
            ILogger<InteractionClientManager> logger,
            DaprClient daprClient,
            RegisterCoreInteractionConfiguration.AddInteractionDependenciesOptions addInteractionDependenciesOptions)
        {
            _logger = logger;
            _daprClient = daprClient;

            _apiPrefix = addInteractionDependenciesOptions.ApiPrefix.TrimStart('/').TrimEnd('/');
        }

        public Task InvokeMethodAsync(HttpMethod method, AppId appId, string methodName,
            CancellationToken cancellationToken)
        {
            return InvokeMethodAsync<object?>(method, appId, methodName, null, cancellationToken);
        }

        public Task<TResponse> InvokeMethodAsync<TResponse>(HttpMethod method, AppId appId, string methodName,
            CancellationToken cancellationToken)
        {
            return InvokeMethodAsync<object?, TResponse>(method, appId, methodName, null, cancellationToken);
        }

        public async Task InvokeMethodAsync<TRequest>(HttpMethod method, AppId appId, string methodName,
            TRequest data,
            CancellationToken cancellationToken)
        {
            var response = await _daprClient.InvokeMethodWithResponseAsync(
                GetHttpRequestMessage(method, appId, methodName, data), cancellationToken);

            await HandleHttpResponseError(method, appId, methodName, response, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(responseContent, method, appId, methodName);

                throw new Exceptions.InvocationException(
                    appId: appId,
                    response.StatusCode,
                    responseContent);
            }
        }

        public async Task<TResponse> InvokeMethodAsync<TRequest, TResponse>(HttpMethod method, AppId appId, string methodName,
            TRequest data,
            CancellationToken cancellationToken)
        {

            var response = await _daprClient.InvokeMethodWithResponseAsync(
                GetHttpRequestMessage(method, appId, methodName, data), cancellationToken);

            await HandleHttpResponseError(method, appId, methodName, response, cancellationToken);

            try
            {
                return (await response.Content.ReadFromJsonAsync<TResponse>(new JsonSerializerOptions(), cancellationToken))!;
            }
            catch (JsonException ex)
            {
                throw new Exceptions.InvocationException(
                    appId: appId,
                    response.StatusCode,
                    ex.Message,
                    ex);
            }
        }

        private HttpRequestMessage GetHttpRequestMessage<TRequest>(HttpMethod method, AppId appId, string methodName, TRequest data)
        {
            var request = _daprClient.CreateInvokeMethodRequest(method, appId.GetAppId(), $"{_apiPrefix}/{methodName}", data);

            if (IdentityState.Current != null)
            {
                request.Headers.Add(HeaderNames.UserId, IdentityState.Current.UserId.ToString());
                request.Headers.Add(HeaderNames.TenantId, IdentityState.Current.TenantId?.ToString());
            }

            if (Activity.Current != null)
            {
                var headers = Activity.Current.GetRequestHeaders();
                foreach (var keyValuePair in headers)
                {
                    request.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return request;
        }

        private async Task HandleHttpResponseError(HttpMethod method, AppId appId, string methodName,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError(responseContent, method, appId, methodName);
                
                throw new Exceptions.InvocationException(
                    appId: appId,
                    response.StatusCode,
                    responseContent);
            }
        }
    }
}
