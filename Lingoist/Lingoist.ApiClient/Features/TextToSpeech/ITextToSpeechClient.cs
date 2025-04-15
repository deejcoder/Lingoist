using Lingoist.ApiClient.Features.TextToSpeech.Models;

namespace Lingoist.ApiClient.Features.TextToSpeech
{
    public interface ITextToSpeechClient
    {
        IAsyncEnumerable<byte[]> SynthesizeAsync(TextToSpeechRequest request, CancellationToken? cancellationToken = null);
        Task<byte[]?> GetHistory(CancellationToken? cancellationToken = null);
    }
}
