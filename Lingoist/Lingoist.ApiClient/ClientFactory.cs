using Lingoist.ApiClient.Features.TextToSpeech;

namespace Lingoist.ApiClient
{
    public class ClientFactory
    {
        private readonly HttpClient HttpClient;

        public ClientFactory(string baseUrl)
        {
            // TODO add auth
            HttpClient = new();
            HttpClient.BaseAddress = new Uri(baseUrl);
        }

        public ITextToSpeechClient CreateTextToSpeechClient()
        {
            return new TextToSpeechClient(HttpClient);
        }
    }
}
