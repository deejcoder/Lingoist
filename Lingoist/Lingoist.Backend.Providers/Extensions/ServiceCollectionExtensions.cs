using Lingoist.Backend.Providers.Contract;
using Lingoist.Backend.Providers.ElevenLabs;
using Microsoft.Extensions.DependencyInjection;

namespace Lingoist.Backend.Providers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequiredProviderServices(this IServiceCollection services)
    {
        services.AddSingleton<ProviderClientFactory>();
        return services;
    }

    public static IServiceCollection AddTextToSpeech(this IServiceCollection services, string apiKey)
    {
        services.AddSingleton<ITextToSpeechClient>(sp => new EleventLabsTextToSpeechApiClient(apiKey));

        return services;
    }
}
