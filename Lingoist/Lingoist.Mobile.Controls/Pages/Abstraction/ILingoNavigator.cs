using Lingoist.Mobile.UI.Pages.Navigation;

namespace Lingoist.Mobile.UI.Pages.Abstraction
{
    public interface ILingoNavigator
    {
        void Attach(LingoPagedLayout host);
        void Detach();

        Task NavigateToAsync<TPage>(LingoistNavigationOptions options) where TPage : class, ILingoPage;
    }
}
