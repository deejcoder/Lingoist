using Lingoist.Mobile.UI.Pages.Abstraction;

namespace Lingoist.Mobile.Pages;

public partial class AddEditPracticeSetPage : ContentView, ILingoStatefulPage<AddEditPracticeSetPageState>
{
	private int CurrentIndex = 0;

	private readonly ILingoNavigator Navigator;
	public string ContentDisplay { get; set; } = string.Empty;


	public AddEditPracticeSetPage(ILingoNavigator navigator)
	{
		this.Navigator = navigator;

		InitializeComponent();
	}

	~AddEditPracticeSetPage()
	{
		// If this is called, this proves there is no memory leak, at least at the page level.
	}

	public void Setup(AddEditPracticeSetPageState state)
	{
		CurrentIndex = state.Index;
		ContentDisplay = $"Current index: {CurrentIndex}";
		OnPropertyChanged(nameof(ContentDisplay));

		layout.HideFooter(animate: false);

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
		// Pick a random animation
		LingoPageAnimation[] possibleAnimations = [
			LingoPageAnimation.SlideFromRight,
			LingoPageAnimation.SlideFromBottom,
		];

		LingoPageAnimation animation = possibleAnimations[new Random().Next(0, possibleAnimations.Length)];

		// Just to test for now, we should be able to infinitely go forward
		AddEditPracticeSetPageState state = new()
		{
			Index = CurrentIndex + 1
		};

		this.Navigator.NavigateToAsync<AddEditPracticeSetPage, AddEditPracticeSetPageState>(state, animation);
	}

	protected override void OnParentSet()
	{
		base.OnParentSet();
	}

	private void OnCheckClicked(object sender, EventArgs e)
	{
		layout.ShowFooter(overlay: true);
	}

    private void OnPreviousClicked(object sender, EventArgs e)
    {
		this.Navigator.NavigateBackAsync();
    }
}