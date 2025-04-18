using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Navigation;

namespace Lingoist.Mobile.Pages;

public partial class AddEditPracticeSetPage : ContentView, ILingoPage
{
	private readonly ILingoNavigator Navigator;

	public AddEditPracticeSetPage(ILingoNavigator navigator)
	{
		this.Navigator = navigator;

        InitializeComponent();
	}

    public void Setup()
    {
        layout.IsFooterVisible = false;

        string[] darkShades = [
            "#333333",
            "#2C2C2C",
            "#1F1F1F",
            "#3B3B3B",
            "#2B2B2B",
            "#141414",
        ];

        // pick a random dark shade and set it as the background
        string randomDarkShade = darkShades[new Random().Next(0, darkShades.Length)];
        this.BackgroundColor = Color.FromArgb(randomDarkShade);
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
		// Just to test for now, we should be able to infinitely go forward
		this.Navigator.NavigateToAsync<AddEditPracticeSetPage>(new LingoistNavigationOptions()
		{
			Recycle = true
		});
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        // Just to test footer animations
        if(this.Parent != null)
        {
            Dispatcher.Dispatch(async () =>
            {
                await Task.Delay(2000);

                this.layout.IsFooterVisible = true;
            });
        }
    }
}