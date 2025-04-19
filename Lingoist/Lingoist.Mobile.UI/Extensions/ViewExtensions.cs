namespace Lingoist.Mobile.UI.Extensions
{
    public static class ViewExtensions
    {
        /// <summary>
        /// Destroys a view when it is no longer required
        /// </summary>
        /// <param name="view"></param>        
        public static void DestroyView(this IView view)
        {
            // get rid of the deepest descendants first
            foreach (var descendant in GetDescendants(view))
            {
                try
                {
                    // 1. Disconnect the handler
                    descendant.Handler?.DisconnectHandler();

                    // 2. If the view is disposable, dispose it
                    if (descendant is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }

                    // 3. Clear the binding context.
                    // The first three steps may require the binding context
                    if (descendant is BindableObject bindable)
                    {
                        bindable.BindingContext = null;
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Gets all descendants of a view, starting with the deepest children
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IEnumerable<IView> GetDescendants(IView view)
        {
            if (view is IVisualTreeElement vte)
            {
                foreach (var child in vte.GetVisualChildren())
                {
                    if (child is IView childView)
                    {
                        foreach (var descendant in GetDescendants(childView))
                        {
                            yield return descendant;
                        }

                        yield return childView;
                    }
                }
            }

            yield return view;
        }
    }
}
