namespace Lingoist.Mobile.UI.Pages.Abstraction
{
    /// <summary>
    /// Represents a page with state
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface ILingoStatefulPage<TState> : ILingoPage
        where TState : class, new()
    {
        void Setup(TState state);
    }
}
