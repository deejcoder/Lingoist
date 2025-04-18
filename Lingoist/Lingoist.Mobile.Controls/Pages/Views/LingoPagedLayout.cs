using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Views;
using Microsoft.Maui.Layouts;

namespace Lingoist.Mobile.UI.Pages;

/// <summary>
/// A layout used to render pages with custom animations and page recycling
/// </summary>
public class LingoPagedLayout : Layout, IDisposable
{
    private List<ILingoPage> _pages = [];

    // Navigator bindable property
    public static readonly BindableProperty NavigatorProperty =
        BindableProperty.Create(nameof(Navigator), typeof(ILingoNavigator), typeof(LingoPagedLayout), null, propertyChanged: NavigatorPropertyChanged);

    public ILingoNavigator? Navigator
    {
        get => (ILingoNavigator)GetValue(NavigatorProperty);
        set => SetValue(NavigatorProperty, value);
    }

    private static void NavigatorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LingoPagedLayout view)
        {
            if (oldValue is ILingoNavigator oldNavigator)
            {
                oldNavigator.Detach();
            }
            if (newValue is ILingoNavigator newNavigator)
            {
                newNavigator.Attach(view);
            }
        }
    }

    public LingoPagedLayout()
    {
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return new LingoPagedLayoutManager(this);
    }

    internal ILingoPage? RecyclePreviousPage(Type pageType)
    {
        // check the stack for a page of the given type, that is not at the top
        if (_pages.Count > 0)
        {
            // ignore the last page, as it is the current page
            for (int i = 0; i < _pages.Count - 1; i++)
            {
                var page = _pages[i];
                if (page.GetType() == pageType)
                {
                    _pages.RemoveAt(i);
                    return page;
                }
            }
        }

        return null;
    }

    internal async Task MoveNextAsync(ILingoPage next, LingoPageAnimation animation, bool isRecycled = false)
    {
        ILingoPage? current = _pages.LastOrDefault();

        if (next is View nextView)
        {
            switch (animation)
            {
                case LingoPageAnimation.None:
                    {
                        nextView.ZIndex = 1000;
                        nextView.IsVisible = true;

                        if (current != null && current is View currentView)
                        {
                            currentView.IsVisible = false;
                        }
                    }
                    break;

                case LingoPageAnimation.SlideFromRight:
                    {
                        var width = this.Width;

                        nextView.TranslationX = width;

                        if (isRecycled)
                        {
                            // we need to move the page last, this is mostly so events like OnParentSet are still fired
                            this.Children.Remove(next);
                            this.Children.Add(next);
                        }
                        else
                        {
                            this.Children.Add(next);
                        }

                        nextView.ZIndex = 1000;
                        nextView.IsVisible = true;
                        await nextView.TranslateTo(0, 0, 400, Easing.CubicOut);

                        if (current != null && current is View currentView)
                        {
                            //await currentView.TranslateTo(-width / 2, 0, 300, Easing.CubicIn);
                            currentView.IsVisible = false;
                        }
                    }
                    break;
            }

            nextView.ZIndex = 0;
        }

        _pages.Add(next);
    }

    public void Dispose()
    {
        if (Navigator != null) Navigator.Detach();
    }
}