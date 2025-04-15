using Lingoist.Mobile.Application.Features.TextToSpeech;
using Lingoist.Mobile.Plugins.Audio;

namespace Lingoist.Mobile
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly IAudioPlayer Player;
        private readonly TextToSpeechService Tts;

        public MainPage(IAudioPlayer player, TextToSpeechService tts)
        {
            this.Player = player;
            this.Tts = tts;
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async void TranscribeClicked(object sender, EventArgs e)
        {
            try
            {
                byte[] bytes = await Tts.GetHistory(); //Tts.Run("Guten Tag. Wie geht es dir?", "iP95p4xoKVk53GoZ742B");

                // Play the audio
                // Convert the async enumerable to a stream
                using (var stream = new MemoryStream())
                {
                    // Read the bytes from the async enumerable and write them to the stream
                    //foreach (var chunk in bytes)
                    //{

                    //}
                    await stream.WriteAsync(bytes);

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
