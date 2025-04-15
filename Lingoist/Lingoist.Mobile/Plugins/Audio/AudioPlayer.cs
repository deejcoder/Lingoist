
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
                // save to file
                var filePath = Path.Combine(FileSystem.CacheDirectory, "audio.mp3");
                using (var fileStream = File.Create(filePath))

                {
                    await stream.CopyToAsync(fileStream, cancellationToken.Value);
                }

                // Reset the stream position to the beginning
                stream.Position = 0;                                                

                Plugin.Maui.Audio.IAudioPlayer player = this.AudioManager.CreatePlayer(filePath);
                player.Volume = 1.0;
                player.Seek(0);
                player.SetSource(stream);
                player.Play();
                //await player.PlayAsync(cancellationToken.Value);
            }
            catch(Exception ex)
            {

            }

        }
    }
}
