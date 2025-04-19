using Lingoist.Mobile.UI.Pages.Navigation;

namespace Lingoist.Mobile.UI.Pages.Abstraction
{
    public interface ILingoNavigator
    {
        void Attach(LingoPagedLayout host);
        void Detach();

		/// <summary>
		/// Navigates to a stateless lingo page with the given animation and options.
		/// </summary>
		/// <typeparam name="TPage"></typeparam>
		/// <param name="animation">The type of animation, defaults to none</param>
		/// <param name="options"></param>
		/// <returns></returns>
		Task NavigateToAsync<TPage>(LingoPageAnimation animation = LingoPageAnimation.None, LingoNavigationOptions? options = null) 
            where TPage : class, ILingoStatelessPage;

		/// <summary>
		/// Navigates to a stateful lingo page with the given state, animation and options.
		/// </summary>
		/// <typeparam name="TPage"></typeparam>
		/// <typeparam name="TState"></typeparam>
		/// <param name="state"></param>
		/// <param name="animation"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		Task NavigateToAsync<TPage, TState>(TState state, LingoPageAnimation animation = LingoPageAnimation.None, LingoNavigationOptions? options = null)
            where TPage : class, ILingoStatefulPage<TState>
            where TState : class, new();

		/// <summary>
		/// Navigates back to the previous page
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		Task NavigateBackAsync(LingoPageAnimation animation = LingoPageAnimation.SlideFromLeft, LingoNavigationOptions? options = null);
	}
}
