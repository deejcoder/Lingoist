using Lingoist.Mobile.Application.Features.TextToSpeech;
using Lingoist.Mobile.Pages;
using Lingoist.Mobile.Plugins.Audio;
using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Navigation;

namespace Lingoist.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly IAudioPlayer Player;
        private readonly TextToSpeechService Tts;        
        public ILingoNavigator Navigator { get; set; }

        public MainPage(ILingoNavigator navigator, IAudioPlayer player, TextToSpeechService tts)
        {
            this.Navigator = navigator;
            this.Player = player;
            this.Tts = tts;

            InitializeComponent();
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if(Parent != null)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(2000);

                    AddEditPracticeSetPageState state = new()
                    {
                        Index = 0
                    };

					await Navigator.NavigateToAsync<AddEditPracticeSetPage, AddEditPracticeSetPageState>(state, LingoPageAnimation.SlideFromRight);
                });
            }
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
