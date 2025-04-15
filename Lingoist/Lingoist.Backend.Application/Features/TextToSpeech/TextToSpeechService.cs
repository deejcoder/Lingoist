using Lingoist.Backend.Providers;
using Lingoist.Backend.Providers.Contract;
using Lingoist.Backend.Providers.Models;

namespace Lingoist.Backend.Application.Features.TextToSpeech
{
    public class TextToSpeechService
    {
        private readonly ITextToSpeechClient Client;

        public TextToSpeechService(ProviderClientFactory providerClientFactory)
        {
            this.Client = providerClientFactory.GetClient<ITextToSpeechClient>();
        }

        public async Task<List<Voice>> GetVoicesAsync()
        {
            return await Client.GetVoicesAsync();
        }

        public async IAsyncEnumerable<byte[]> TextToSpeech(TextToSpeechRequest request)
        {
            await foreach (var chunk in Client.TextToSpeech(request))
            {
                yield return chunk;
            }
        }

        public async Task<byte[]?> UpdateHistory()
        {
            return await Client.UpdateHistory();
        }
    }
}
