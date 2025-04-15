using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lingoist.Backend.Providers.Models
{
    public class VoiceList
    {
        [JsonProperty("voices")]
        public List<Voice> Items { get; set; } = [];
    }
}
