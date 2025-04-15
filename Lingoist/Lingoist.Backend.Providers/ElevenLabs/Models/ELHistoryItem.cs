using Newtonsoft.Json;

namespace Lingoist.Backend.Providers.ElevenLabs.Models
{
    public class ELHistoryItem
    {
        [JsonProperty("history_item_id")]
        public string HistoryItemId { get; set; } = string.Empty;
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
