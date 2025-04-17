namespace Lingoist.Mobile.UI.Pages;

public partial class LingoistSlidablePageHost : ContentView
{
    public static readonly BindableProperty PageProperty =
        BindableProperty.Create(nameof(Page), typeof(ILingoistPage), typeof(LingoistSlidablePageHost), default(ILingoistPage),
            propertyChanged: PagePropertyChanged);

    public ILingoistPage Page
    {
        get => (ILingoistPage)GetValue(PageProperty);
        set => SetValue(PageProperty, value);
    }

    private static void PagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LingoistSlidablePageHost pageHost)
        {
            ILingoistPage? current;
            if (oldValue is ILingoistPage oldPage)
            {
                current = oldPage;                
            }
            else
            {
                current = null;
            }

            if(newValue is ILingoistPage next)
            {
                pageHost.Dispatcher.Dispatch(async () => await pageHost.MoveNextAsync(current, next));
            }            
        }
    }

    public LingoistSlidablePageHost()
    {
        InitializeComponent();
    }

    private async Task MoveNextAsync(ILingoistPage? current, ILingoistPage next)
    {
        var width = HostContainer.Width;

        if (next is View nextView)
        {
            nextView.TranslationX = width;

            HostContainer.Children.Add(next);

            if (current != null && current is View currentView)
            {
                await currentView.TranslateTo(-width / 2, 0, 300, Easing.CubicIn);
                currentView.IsVisible = false;
            }

            nextView.IsVisible = true;
            await nextView.TranslateTo(0, 0, 400, Easing.CubicOut);
        }
    }
}