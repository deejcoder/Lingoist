using Lingoist.ApiClient;
using Lingoist.ApiClient.Features.TextToSpeech;
using Lingoist.ApiClient.Features.TextToSpeech.Models;

namespace Lingoist.Mobile.Application.Features.TextToSpeech
{
    public class TextToSpeechService
    {
        private readonly ITextToSpeechClient Client;

        public TextToSpeechService()
        {
            // Constructor logic here
            ClientFactory factory = new("https://localhost:44368/");
            Client = factory.CreateTextToSpeechClient();
        }

        public IAsyncEnumerable<byte[]> Run(string text, string voiceId)
        {
            var request = new TextToSpeechRequest()
            {
                Text = text,
                VoiceId = voiceId
            };

            return Client.SynthesizeAsync(request);
        }

        public async Task<byte[]> GetHistory()
        {
            return (await Client.GetHistory()) ?? [];
        }
    }
}
