using Lingoist.Mobile.UI.Extensions;

namespace Lingoist.Mobile.UI.LayoutBlocks;

public partial class LingoistLayoutFooter : ContentView
{
    private bool _animateInRequested;

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

            control.FooterContentContainer.IsVisible = isVisible;
            //control.FooterContent.IsVisible = isVisible;

            if (isVisible)
            {
                // wait for the size to be updated before triggering the animation
                control._animateInRequested = true;
                control.AnimateOnAppearing();
            }
            else
            {
                control.AnimateOnDisappearing();
            }
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
            FooterContentContainer.IsVisible = IsFooterVisible;
        }
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
    }

    private void AnimateOnAppearing()
    {
        if (FooterContent == null || Parent == null || !_canAnimateFooter) return;

        if (!EnableAnimations)
            return;

        this.Dispatcher.Dispatch(async () =>
        {
            _animateInRequested = false;

            // Start the footer off-screen (below the visible area)
            if (this.Parent is View parentView)
            {
                FooterContentContainer.TranslationY = parentView.Height;
            }
            FooterContent.IsVisible = true;

            // Animate it to its original position
            await FooterContentContainer.TranslateTo(0, 0, 600, Easing.CubicIn);
        });
    }

    private void AnimateOnDisappearing()
    {
        if (FooterContent == null || Parent == null || !_canAnimateFooter) return;

        if (!EnableAnimations)
            return;

        this.Dispatcher.Dispatch(async () =>
        {
            if (FooterContentContainer != null)
            {
                // Animate the footer to move off-screen (below the visible area)
                await FooterContent.TranslateTo(0, FooterContentContainer.Height, 600, Easing.CubicIn);

                // Optionally, hide the footer after the animation
                FooterContentContainer.IsVisible = false;
            }
        });
    }

    private bool _canAnimateFooter = false;

    private void OnContainerParentChanged(object sender, EventArgs e)
    {
        _canAnimateFooter = true;

        if (_animateInRequested) AnimateOnAppearing();
    }
}