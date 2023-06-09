using IvNav.Store.Core.Interaction.Abstractions.Helpers;
using IvNav.Store.Core.Interaction.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace IvNav.Store.Core.Interaction.Configurations;

public static class RegisterCoreInteractionConfiguration
{
    public class AddInteractionDependenciesOptions
    {
        public string ApiPrefix { get; set; } = "api/v1";

        internal AddInteractionDependenciesOptions()
        {

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
