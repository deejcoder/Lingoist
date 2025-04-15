namespace Lingoist.ApiClient.Features.TextToSpeech.Models;
public class TextToSpeechRequest
{
    public string VoiceId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
