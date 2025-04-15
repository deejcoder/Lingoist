
using Plugin.Maui.Audio;

namespace Lingoist.Mobile.Plugins.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private readonly IAudioManager AudioManager;        

        public AudioPlayer(IAudioManager audioManager)
        {
            this.AudioManager = audioManager;
        }

        public async Task PlayAudioStream(Stream stream, CancellationToken? cancellationToken = null)
        {
            cancellationToken ??= CancellationToken.None;

            try
            {
                AsyncAudioPlayer player = this.AudioManager.CreateAsyncPlayer(stream, new AudioPlayerOptions());
                await player.PlayAsync(cancellationToken.Value);
            }
            catch(Exception ex)
            {

            }

        }
    }
}
