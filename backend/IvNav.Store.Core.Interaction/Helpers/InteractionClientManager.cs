using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Dapr.Client;
using IvNav.Store.Common.Extensions;
using IvNav.Store.Common.Identity;
using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Configurations;
using IvNav.Store.Core.Interaction.Enums;
using IvNav.Store.Core.Interaction.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace IvNav.Store.Core.Interaction.Helpers
{
    internal class InteractionClientManager : IInteractionClientManager
    {
        private readonly ILogger<InteractionClientManager> _logger;
        private readonly DaprClient _daprClient;

        private readonly RegisterCoreInteractionConfiguration.AddInteractionDependenciesOptions _options;

        public InteractionClientManager(
            ILogger<InteractionClientManager> logger,
            DaprClient daprClient,
            RegisterCoreInteractionConfiguration.AddInteractionDependenciesOptions options)
        {
            _logger = logger;
            _daprClient = daprClient;
            _options = options;
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

            cancellationToken.ThrowIfCancellationRequested();

            await HandleHttpResponseError(method, appId, methodName, response, cancellationToken);
        }

        public async Task<TResponse> InvokeMethodAsync<TRequest, TResponse>(HttpMethod method, AppId appId, string methodName,
            TRequest data,
            CancellationToken cancellationToken)
        {

            var response = await _daprClient.InvokeMethodWithResponseAsync(
                GetHttpRequestMessage(method, appId, methodName, data), cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

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
            var request = _daprClient.CreateInvokeMethodRequest(method, appId.GetAppId(), $"{_options.GetApiPrefix(appId)}/{methodName}", data);

            if (IdentityState.Current != null && !string.IsNullOrEmpty(IdentityState.Current.BearerToken))
            {
                request.Headers.Add(HeaderNames.Authorization, $"Bearer {IdentityState.Current.BearerToken}");
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
                _logger.LogError("HandleHttpResponseError {responseContent} {method} {appId} {methodName}", responseContent, method, appId, methodName);
                
                throw new Exceptions.InvocationException(
                    appId: appId,
                    response.StatusCode,
                    responseContent);
            }
        }
    }
}
