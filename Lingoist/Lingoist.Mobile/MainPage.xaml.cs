using Lingoist.Mobile.Application.Features.TextToSpeech;
using Lingoist.Mobile.Pages;
using Lingoist.Mobile.Plugins.Audio;
using Lingoist.Mobile.UI.Pages;

namespace Lingoist.Mobile
{
    public partial class MainPage : LingoistPage
    {
        private readonly IAudioPlayer Player;
        private readonly TextToSpeechService Tts;
        public LingoistPageHost PageHost { get; set; } = new();

        public MainPage(IAudioPlayer player, TextToSpeechService tts)
        {
            this.Player = player;
            this.Tts = tts;
            InitializeComponent();

            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(2000);

                await PageHost.NavigateTo<AddEditPracticeSetPage>(null);
            });
        }

        private async void TranscribeClicked(object sender, EventArgs e)
        {
            try
            {
                IAsyncEnumerable<byte[]> bytes = Tts.Run("Guten Tag. Ich bin gut", "iP95p4xoKVk53GoZ742B");

                // Play the audio
                // Convert the async enumerable to a stream
                using (var stream = new MemoryStream())
                {
                    // Read the bytes from the async enumerable and write them to the stream
                    await foreach (var chunk in bytes)
                    {
                        await stream.WriteAsync(chunk);
                    }
                    // Reset the stream position to the beginning
                    stream.Position = 0;
                    // Play the audio from the stream
                    await Player.PlayAudioStream(stream);
                }
            }
            catch(Exception ex)
            {

            }

        }
    }

}
