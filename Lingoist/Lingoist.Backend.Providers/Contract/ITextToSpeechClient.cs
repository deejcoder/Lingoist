using Lingoist.Backend.Providers.Models;

namespace Lingoist.Backend.Providers.Contract;

public interface ITextToSpeechClient : IProviderClient
{
    Task<List<Voice>> GetVoicesAsync();
    IAsyncEnumerable<byte[]> TextToSpeech(TextToSpeechRequest request);
    Task<byte[]?> UpdateHistory();
}
