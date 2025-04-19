using Lingoist.Mobile.UI.Extensions;

namespace Lingoist.Mobile.UI.Layouts;

public partial class LingoistLayoutBase : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(LingoistLayoutBase), default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty LayoutContentProperty =
        BindableProperty.Create(nameof(LayoutContent), typeof(View), typeof(LingoistLayoutBase), default(View),
            propertyChanged: LayoutContentPropertyChanged);

    public View LayoutContent
    {
        get => (View)GetValue(LayoutContentProperty);
        set => SetValue(LayoutContentProperty, value);
    }

    private static void LayoutContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
    }

    public static readonly BindableProperty FooterContentProperty =
        BindableProperty.Create(nameof(FooterContent), typeof(View), typeof(LingoistLayoutBase), default(View));

    public View FooterContent
    {
        get => (View)GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    public static readonly BindableProperty IsFooterVisibleProperty =
        BindableProperty.Create(nameof(IsFooterVisible), typeof(bool), typeof(LingoistLayoutBase), default(bool));

    public bool IsFooterVisible
    {
        get => (bool)GetValue(IsFooterVisibleProperty);
        set => SetValue(IsFooterVisibleProperty, value);
    }

    public LingoistLayoutBase()
    {
        InitializeComponent();
    }

    public async void ShowFooter(bool overlay = false, bool animate = true)
    {        
        if (overlay)
        {
            // shift the footer to the content area, if showing it as an overlay
            Grid.SetRow(LingoistLayoutFooter, 1);
        }

        await LingoistLayoutFooter.ShowAsync(overlay, animate);
    }

    public async void HideFooter(bool animate = true)
    {
        await LingoistLayoutFooter.HideAsync(animate);

        // reset the row after the animation has finished
        Grid.SetRow(LingoistLayoutFooter, 2);
    }
}