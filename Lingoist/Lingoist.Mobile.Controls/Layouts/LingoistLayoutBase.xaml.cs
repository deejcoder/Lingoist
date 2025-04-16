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
        if(bindable is LingoistLayoutBase layout)
        {
            layout.DestroyLayoutContent();
            layout.ApplyLayoutContent();
        }
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

    private void DestroyLayoutContent()
    {
        if (this.LayoutContentContainer != null)
        {
            this.LayoutContentContainer.DestroyView();
            this.LayoutContentContainer = null;
        }
    }

    private void ApplyLayoutContent()
    {
        if (LayoutContent != null)
        {
            this.LayoutContentContainer.Content = LayoutContent;

            // forward the binding context
            LayoutContent.BindingContext = this.BindingContext;
        }
        else
        {
            this.LayoutContentContainer.Content = new VerticalStackLayout();
        }
    }
}