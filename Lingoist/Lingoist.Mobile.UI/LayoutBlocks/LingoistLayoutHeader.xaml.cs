namespace Lingoist.Mobile.UI.LayoutBlocks;

public partial class LingoistLayoutHeader : ContentView
{    
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(LingoistLayoutHeader), default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public LingoistLayoutHeader()
	{
		InitializeComponent();
	}
}