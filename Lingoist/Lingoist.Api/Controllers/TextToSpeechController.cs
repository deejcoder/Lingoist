using Lingoist.Backend.Application.Features.TextToSpeech;
using Lingoist.Backend.Providers.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Lingoist.Api.Controllers
{    
    public class TextToSpeechController : Controller
    {
        private readonly ILogger<TextToSpeechController> Logger;
        private readonly TextToSpeechService TextToSpeechService;

        public TextToSpeechController(ILogger<TextToSpeechController> logger, TextToSpeechService textToSpeechService)
        {
            this.Logger = logger;
            this.TextToSpeechService = textToSpeechService;
        }
        [HttpGet("voices")]
        public async Task<IActionResult> GetVoices()
        {
            var voices = await TextToSpeechService.GetVoicesAsync();
            return Ok(voices);
        }

        [HttpPost("synthesize")]
        public async IAsyncEnumerable<string> Synthesize([FromBody] TextToSpeechRequest request)
        {
            await foreach (var chunk in TextToSpeechService.TextToSpeech(request))
            {
                yield return Convert.ToBase64String(chunk);
            }
        }

        [HttpPost("update-history")]
        public async Task<string> UpdateHistory()
        {
            byte[]? bytes = await TextToSpeechService.UpdateHistory();
            bytes ??= [];

            return Convert.ToBase64String(bytes);
        }
    }
}
