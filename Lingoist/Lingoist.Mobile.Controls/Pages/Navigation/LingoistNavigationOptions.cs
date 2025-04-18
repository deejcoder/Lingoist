using Lingoist.Mobile.UI.Pages.Abstraction;

namespace Lingoist.Mobile.UI.Pages.Navigation
{
    public class LingoistNavigationOptions
    {
        public LingoPageAnimation Animation { get; set; } = LingoPageAnimation.SlideFromRight;
        public bool Recycle { get; set; }
    }
}
