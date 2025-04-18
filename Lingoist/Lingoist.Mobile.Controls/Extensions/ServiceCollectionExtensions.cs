using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Navigation;
using Lingoist.Mobile.UI.Pages.Registry;

namespace Lingoist.Mobile.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddLingoNavigation(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ILingoNavigator, LingoNavigator>();
        }

        public static void AddLingoPage<TPage>(this MauiAppBuilder builder) where TPage : class, ILingoPage
        {
            // Register the page with the service collection
            builder.Services.AddTransient<TPage>();

            // Register the page with the registry
            LingoistPageRegistry.Register<TPage>();
        }
    }
}
