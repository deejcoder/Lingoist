using Lingoist.Mobile.UI.Extensions;

namespace Lingoist.Mobile.UI.LayoutBlocks;

public partial class LingoistLayoutFooter : ContentView
{
    public static readonly BindableProperty FooterContentProperty =
        BindableProperty.Create(nameof(FooterContent), typeof(View), typeof(LingoistLayoutFooter), null, propertyChanged: FooterContentPropertyChanged);

    public View FooterContent
    {
        get => (View)GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    private static void FooterContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (LingoistLayoutFooter)bindable;

        if (oldValue is View)
        {
            control.DestroyFooterContent();
        }

        if (newValue is View)
        {
            control.ApplyFooterContent();
        }
    }

    public static readonly BindableProperty IsFooterVisibleProperty =
        BindableProperty.Create(nameof(IsFooterVisible), typeof(bool), typeof(LingoistLayoutFooter), true, propertyChanged: IsFooterVisibleChanged);

    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    private static void IsFooterVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (LingoistLayoutFooter)bindable;
        if (control.FooterContent != null)
        {
            bool isVisible = (bool)newValue;

            if (isVisible)
            {
                _ = control.ShowAsync();
            }
            else
            {
                _ = control.HideAsync();
            }
        }
    }

    public LingoistLayoutFooter()
    {
        InitializeComponent();
    }

    private void DestroyFooterContent()
    {
        if (FooterContent != null)
        {
            FooterContentContainer.Content.DestroyView();
            FooterContentContainer.Content = null;
        }
    }

    private void ApplyFooterContent()
    {
        if (FooterContent != null)
        {
            FooterContentContainer.Content = FooterContent;
            FooterContentContainer.Content.IsVisible = IsFooterVisible;
            this.IsVisible = IsFooterVisible;
        }
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
    }

    public async Task ShowAsync(bool overlay = false, bool animate = true)
    {
        if (FooterContent == null || Parent == null) return;

        await this.Dispatcher.DispatchAsync(async () =>
        {
            if (overlay) FooterContentContainer.ZIndex = 1002;
            this.IsVisible = true;

            if(animate)
            {
                // Start the footer off-screen (below the visible area)
                if (this.Parent is View parentView)
                {
                    FooterContentContainer.TranslationY = parentView.Height;
                }
            }

            FooterContent.IsVisible = true;

            if(animate)
            {
                // Animate it to its original position
                await FooterContentContainer.TranslateTo(0, 0, 600, Easing.BounceIn);
            }
        });
    }

    public async Task HideAsync(bool animate = true)
    {
        if (FooterContent == null || Parent == null) return;

        await this.Dispatcher.DispatchAsync(async () =>
        {
            if(animate)
            {
                // Animate the footer to move off-screen (below the visible area)
                if (this.Parent is View parentView)
                {
                    await FooterContentContainer.TranslateTo(0, parentView.Height, 600, Easing.BounceOut);
                }
            }

            this.IsVisible = false;
            FooterContentContainer.ZIndex = 0;
        });
    }
}