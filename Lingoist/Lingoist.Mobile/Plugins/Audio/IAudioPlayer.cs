namespace Lingoist.Mobile.Plugins.Audio
{
    public interface IAudioPlayer
    {
        Task PlayAudioStream(Stream stream, CancellationToken? cancellationToken = null);
    }
}
