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
            control.FooterContent.IsVisible = (bool)newValue;
        }
    }

    public static readonly BindableProperty EnableAnimationsProperty =
        BindableProperty.Create(nameof(EnableAnimations), typeof(bool), typeof(LingoistLayoutFooter), true);

    public bool EnableAnimations
    {
        get => (bool)GetValue(EnableAnimationsProperty);
        set => SetValue(EnableAnimationsProperty, value);
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
        }
    }

    private void AnimateOnAppearing()
    {
        if (!EnableAnimations)
            return;

        this.Dispatcher.Dispatch(async () =>
        {
            // Start the footer off-screen (below the visible area)
            FooterContentContainer.TranslationY = FooterContentContainer.Height;

            // Animate it to its original position
            await FooterContentContainer.TranslateTo(0, 0, 500, Easing.CubicOut);
        });
    }

    private void AnimateOnDisappearing()
    {
        if (!EnableAnimations)
            return;

        this.Dispatcher.Dispatch(async () =>
        {
            if (FooterContentContainer != null)
            {
                // Animate the footer to move off-screen (below the visible area)
                await FooterContentContainer.TranslateTo(0, FooterContentContainer.Height, 500, Easing.CubicIn);

                // Optionally, hide the footer after the animation
                FooterContentContainer.IsVisible = false;
            }
        });
    }
}