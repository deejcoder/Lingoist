using Lingoist.Mobile.UI.Pages.Abstraction;
using Microsoft.Extensions.Logging;

namespace Lingoist.Mobile.UI.Pages.Navigation
{
    internal sealed class LingoNavigator : ILingoNavigator, IDisposable
    {
        private WeakReference<LingoPagedLayout>? _host;        

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger<LingoNavigator> Logger;


        public LingoNavigator(IServiceProvider serviceProvider, ILogger<LingoNavigator> logger)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = logger;
        }

        ~LingoNavigator()
        {

        }

        public void Attach(LingoPagedLayout host)
        {
            _host = new WeakReference<LingoPagedLayout>(host);            
        }

        public void Detach()
        {
            _host = null;            
        }

        public async Task NavigateToAsync<TPage>(LingoistNavigationOptions options) where TPage : class, ILingoPage
        {
            await NavigateToAsync(typeof(TPage), options);
        }

        private async Task NavigateToAsync(Type pageType, LingoistNavigationOptions options)
        {
            if (_host == null)
            {
                throw new InvalidOperationException("Host is not attached.");
            }

            if (!_host.TryGetTarget(out LingoPagedLayout? host) || host == null)
            {
                throw new InvalidOperationException("Host is not currently available");
            }

            ILingoPage? page = null;

            bool isRecycled = false;

            if (options.Recycle)
            {
                page = host.RecyclePreviousPage(pageType);
                isRecycled = page != null;
            }

            if (page == null)
            {
                page = (ILingoPage?)this.ServiceProvider.GetService(pageType);
                if (page == null)
                {
                    // fallback and try create an instance of the constructor has no parameters
                    page = (ILingoPage?)Activator.CreateInstance(pageType);

                    if (page == null)
                    {
                        throw new InvalidOperationException($"Cannot create an instance of {pageType.Name}. Please check that the page is registered in the DI container, or has an empty constructor.");
                    }
                }
            }

            page.Setup();

            await host.MoveNextAsync(page, options.Animation, isRecycled);
        }

        public void Dispose()
        {
            Detach();
        }
    }
}
