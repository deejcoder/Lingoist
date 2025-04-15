using Lingoist.Mobile.Application.Features.TextToSpeech;
using Lingoist.Mobile.Plugins.Audio;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace Lingoist.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.AddAudio();
            builder.Services.AddSingleton<Lingoist.Mobile.Plugins.Audio.IAudioPlayer, Plugins.Audio.AudioPlayer>();
            builder.Services.AddTransient<TextToSpeechService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
