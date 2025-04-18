using Lingoist.Mobile.UI.Extensions;
using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Models;
using Lingoist.Mobile.UI.Pages.Registry;
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

    internal ILingoPage? GetPreviousPage()
    {
        if (_pages.Count > 1)
        {
            for(int idx = _pages.Count - 2; idx > 0; idx--)
            {
                ILingoPage page = _pages[idx];

                LingoPageDescriptor descriptor = LingoPageRegistry.GetPageDescriptor(page.GetType());

                // recyclable pages do not support back navigation
                if(descriptor.Recycle)
                {
                    continue;
                }

                return page;
            }
        }

        return null;
    }

    internal async Task MoveToAsync(ILingoPage next, LingoPageAnimation animation)
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

                        this.Children.Add(next);

                        nextView.ZIndex = 1000;
                        nextView.IsVisible = true;
                        await nextView.TranslateTo(0, 0, 400, Easing.CubicOut);

                        if (current != null && current is View currentView)
                        {
                            currentView.IsVisible = false;
                        }
                    }
                    break;

                case LingoPageAnimation.SlideFromBottom:
                    {
                        var height = this.Height;

                        nextView.TranslationY = height;

                        this.Children.Add(next);

                        nextView.ZIndex = 1000;
                        nextView.IsVisible = true;

                        await nextView.TranslateTo(0, 0, 400, Easing.CubicOut);

                        if (current != null && current is View currentView)
                        {
                            currentView.IsVisible = false;
                        }
                    }
                    break;

                case LingoPageAnimation.SlideFromLeft:
                    {
                        var width = this.Width;

                        nextView.TranslationX = -width;

                        this.Children.Add(next);

                        nextView.ZIndex = 1000;
                        nextView.IsVisible = true;

                        await nextView.TranslateTo(0, 0, 400, Easing.CubicOut);

                        if (current != null && current is View currentView)
                        {
                            currentView.IsVisible = false;
                        }
                    }
                    break;
            }

            nextView.ZIndex = 0;

            // remove the previous page now that navigation has completed
            if (current != null)
            {
                this.Children.Remove(current);

                if (current is View currentView)
                {
                    currentView.BindingContext = null;
                }
            }
        }

        if (!_pages.Contains(next))
        {
            _pages.Add(next);
        }

        // if "next" is a previous page, destroy future pages
        if (_pages.IndexOf(next) < _pages.Count - 1)
        {
            DestroyFuturePages(next);
        }
    }

    private void DestroyFuturePages(ILingoPage current)
    {
        int currentIndex = _pages.IndexOf(current);

        // remove all pages after the current page
        for (int i = _pages.Count - 1; i > currentIndex; i--)
        {
            var page = _pages[i];            
            page.DestroyView();
            _pages.Remove(page);
        }
    }

    public void Dispose()
    {
        if (Navigator != null) Navigator.Detach();
    }
}