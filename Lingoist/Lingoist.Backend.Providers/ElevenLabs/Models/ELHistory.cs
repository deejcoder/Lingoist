using Newtonsoft.Json;

namespace Lingoist.Backend.Providers.ElevenLabs.Models
{
    public class ELHistory
    {
        [JsonProperty("history")]
        public List<ELHistoryItem> Items { get; set; } = [];
    }
}
