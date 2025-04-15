using Lingoist.Backend.Providers.Contract;
using Lingoist.Backend.Providers.ElevenLabs.Models;
using Lingoist.Backend.Providers.Models;
using Newtonsoft.Json;
using System.Text;

namespace Lingoist.Backend.Providers.ElevenLabs;

internal sealed class EleventLabsTextToSpeechApiClient : ITextToSpeechClient
{
    private const string BaseUrl = "https://api.elevenlabs.io/v1/";

    private readonly HttpClient _httpClient;
    public EleventLabsTextToSpeechApiClient(string apiKey)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Add("xi-api-key", apiKey);
    }

    public async Task<List<Voice>> GetVoicesAsync()
    {
        var response = await _httpClient.GetAsync("voices");
        response.EnsureSuccessStatusCode();
        VoiceList? voices = JsonConvert.DeserializeObject<VoiceList>(await response.Content.ReadAsStringAsync());
        return (voices?.Items ?? []).ToList();
    }


    public async IAsyncEnumerable<byte[]> TextToSpeech(TextToSpeechRequest request)
    {
        Dictionary<string, string> values = [];
        values.Add("text", request.Text);
        values.Add("model_id", "eleven_multilingual_v2");
        var json = JsonConvert.SerializeObject(values);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //content.Headers.ContentType.CharSet = "utf-8";
        //content.Headers.Add("Accept", "audio/mpeg");

        var response = await _httpClient.PostAsync("text-to-speech/" + request.VoiceId + "/stream?output_format=mp3_44100_128", content);
        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();
        byte[] buffer = new byte[8192]; // 8 KB buffer size
        int bytesRead;

        while ((bytesRead = await responseStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            yield return buffer.Take(bytesRead).ToArray();
        }
    }

    public async Task<byte[]?> UpdateHistory()
    {
        var response = await _httpClient.GetAsync("history?page_size=1");
        response.EnsureSuccessStatusCode();

        ELHistory? history = await Deserialize<ELHistory>(response);

        byte[]? lastBytes = null;

        if (history != null)
        {
            foreach (ELHistoryItem item in history.Items)
            {
                Console.WriteLine($"Getting audio for {item.Text}");
                lastBytes = await GetHistoryItemAudio(item.HistoryItemId);
            }
        }

        return lastBytes;
    }

    public async Task<byte[]?> GetHistoryItemAudio(string historyItemId)
    {
        var response = await _httpClient.GetAsync($"history/{historyItemId}/audio");

        // get the raw bytes
        var bytes = await response.Content.ReadAsByteArrayAsync();

        // save to file
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{historyItemId}.mp3");
        await File.WriteAllBytesAsync(filePath, bytes);
        Console.WriteLine($"Audio saved to {filePath}");

        return bytes;
    }

    private async Task<TModel?> Deserialize<TModel>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TModel>(content);
    }
}
