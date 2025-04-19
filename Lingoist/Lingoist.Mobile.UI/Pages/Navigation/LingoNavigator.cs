using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Models;
using Lingoist.Mobile.UI.Pages.Registry;
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

        public async Task NavigateToAsync<TPage>(LingoPageAnimation animation = LingoPageAnimation.SlideFromRight, LingoNavigationOptions? options = null)
            where TPage : class, ILingoStatelessPage
        {
            LingoPagedLayout host = GetHost();

            options ??= new LingoNavigationOptions();

            TPage? page = null;

            // get the page from the registry 
            LingoPageDescriptor descriptor = LingoPageRegistry.GetPageDescriptor(typeof(TPage));

            // check if the page can be recycled and if so, try recycle it
            if (descriptor.Recycle)
            {
                page = (TPage?)host.RecyclePreviousPage(typeof(TPage));
            }

            if (page == null)
            {
                page = (TPage)CreatePage(host, typeof(TPage), options);
            }

            // page is stateless, setup without state
            page.Setup();

            await host.MoveToAsync(page, animation);
        }

        public async Task NavigateToAsync<TPage, TState>(TState state, LingoPageAnimation animation = LingoPageAnimation.SlideFromRight, LingoNavigationOptions? options = null)
            where TPage : class, ILingoStatefulPage<TState>
            where TState : class, new()
        {
            LingoPagedLayout host = GetHost();

            options ??= new LingoNavigationOptions();

            TPage? page = null;

            // get the page from the registry 
            LingoPageDescriptor descriptor = LingoPageRegistry.GetPageDescriptor(typeof(TPage));

            // check if the page can be recycled and if so, try recycle it
            if (descriptor.Recycle)
            {
                page = (TPage?)host.RecyclePreviousPage(typeof(TPage));
            }

            if (page == null)
            {
                page = (TPage)CreatePage(host, typeof(TPage), options);
            }

            // setup the page, pass the state in
            page.Setup(state);

            await host.MoveToAsync(page, animation);
        }

        public async Task NavigateBackAsync(LingoPageAnimation animation = LingoPageAnimation.SlideFromLeft, LingoNavigationOptions? options = null)
        {
            LingoPagedLayout host = GetHost();

            options ??= new LingoNavigationOptions();

            ILingoPage? previousPage = host.GetPreviousPage();

            if (previousPage != null)
            {
                await host.MoveToAsync(previousPage, animation);
            }
        }

        private LingoPagedLayout GetHost()
        {
            if (_host == null)
            {
                throw new InvalidOperationException("Host is not attached.");
            }
            if (!_host.TryGetTarget(out LingoPagedLayout? host) || host == null)
            {
                throw new InvalidOperationException("Host is not currently available");
            }
            return host;
        }

        private ILingoPage CreatePage(LingoPagedLayout host, Type pageType, LingoNavigationOptions options)
        {
            ILingoPage? page = null;

            if (page == null)
            {
                // we can't recycle the page, get a new instance using the DI container
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

            return page;
        }

        public void Dispose()
        {
            Detach();
        }
    }
}
