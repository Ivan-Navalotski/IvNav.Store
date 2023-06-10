using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Enums;
using IvNav.Store.Core.Interaction.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Core.Interaction.Configurations;

public static class RegisterCoreInteractionConfiguration
{
    public class AddInteractionDependenciesOptions
    {
        public IReadOnlyDictionary<AppId, int> ApiVersions { get; }

        internal AddInteractionDependenciesOptions()
        {
            ApiVersions = Enum.GetValues<AppId>().ToDictionary(i => i, _ => 1);
        }

        internal string GetApiPrefix(AppId appId)
        {
            return $"api/v{ApiVersions[appId]}";
        }
    }


    public static IServiceCollection AddInteractionDependencies(this IServiceCollection services,
        Action<AddInteractionDependenciesOptions>? options = null)
    {
        services.AddDaprClient();

        var optionsInternal = new AddInteractionDependenciesOptions();
        options?.Invoke(optionsInternal);

        services.AddSingleton(optionsInternal);
        services.AddSingleton<IInteractionClientManager, InteractionClientManager>();

        return services;
    }
}
