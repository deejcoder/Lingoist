using Lingoist.Backend.Providers.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Lingoist.Backend.Providers;

public sealed class ProviderClientFactory
{
    private readonly IServiceProvider ServiceProvider;

    public ProviderClientFactory(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    public TClient GetClient<TClient>()
        where TClient : class, IProviderClient
    {
        // Can add logic here later to change providers
        TClient? client = ServiceProvider.GetService<TClient>();
        if (client == null)
        {
            throw new InvalidOperationException($"{typeof(TClient).Name} is not registered as a provider client.");
        }

        return client;
    }
}
