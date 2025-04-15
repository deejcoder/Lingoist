using Lingoist.ApiClient.Features.TextToSpeech.Models;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lingoist.ApiClient.Features.TextToSpeech
{
    internal class TextToSpeechClient : ITextToSpeechClient
    {
        private readonly HttpClient HttpClient;

        public TextToSpeechClient(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async IAsyncEnumerable<byte[]> SynthesizeAsync(TextToSpeechRequest request, CancellationToken? cancellationToken = null)
        {
            if (string.IsNullOrWhiteSpace(HttpClient.BaseAddress?.ToString()))
            {
                throw new InvalidOperationException("HttpClient BaseAddress is not set. Please set it before calling this method.");
            }

            cancellationToken ??= CancellationToken.None;

            // Go to the API to stream the audio. It's just a byte array
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/synthesize");

            Uri uri = new Uri(HttpClient.BaseAddress, "/synthesize");
            //requestMessage.Headers.Add("Content-Type", "application/json");

            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            requestMessage.Content = content;

            var response = await HttpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken.Value);

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken.Value);

            // the stream has a base 64 string
            using var reader = new StreamReader(stream);
            using var memoryStream = new MemoryStream();

            byte[] buffer = new byte[8192]; // 8 KB buffer size

            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken.Value)) > 0)
            {
                // Convert the byte array to a base64 string
                string base64String = Convert.ToBase64String(buffer, 0, bytesRead);
                // Convert the base64 string back to a byte array
                byte[] audioBytes = Convert.FromBase64String(base64String);
                yield return audioBytes;
            }
        }

        public async Task<byte[]> GetHistory(CancellationToken? cancellationToken = null)
        {
            cancellationToken ??= CancellationToken.None;
            if (string.IsNullOrWhiteSpace(HttpClient.BaseAddress?.ToString()))
            {
                throw new InvalidOperationException("HttpClient BaseAddress is not set. Please set it before calling this method.");
            }

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/update-history");
            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string base64string = await response.Content.ReadAsStringAsync(cancellationToken.Value);

            if (string.IsNullOrWhiteSpace(base64string))
            {
                return Array.Empty<byte>();
            }

            byte[] bytes = Convert.FromBase64String(base64string);
            return bytes;
        }
    }
}
