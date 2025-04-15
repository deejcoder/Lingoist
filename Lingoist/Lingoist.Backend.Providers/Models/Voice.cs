using Newtonsoft.Json;

namespace Lingoist.Backend.Providers.Models
{    
    public class Voice
    {
        [JsonProperty("voice_id")]
        public string VoiceId { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
