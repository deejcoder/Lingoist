using Microsoft.Maui.Layouts;

namespace Lingoist.Mobile.UI.Pages.Views
{
    /// <summary>
    /// A simple layout manager to render pages
    /// </summary>
    public class LingoPagedLayoutManager : LayoutManager
    {
        private readonly LingoPagedLayout _layout;

        public LingoPagedLayoutManager(LingoPagedLayout layout) : base(layout)
        {
            _layout = layout;
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            IEnumerable<View> children = _layout.Children.OfType<View>()
                .Where(c => c.IsVisible)
                .OrderBy(c => c.ZIndex)
                .AsEnumerable();

            foreach (View child in children)
            {
                child.Arrange(bounds);
            }

            return new Size(bounds.Width, bounds.Height);
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            foreach (IView child in _layout.Children.OfType<View>().Where(c => c.IsVisible))
            {
                child.Measure(widthConstraint, heightConstraint);
            }

            return new Size(widthConstraint, heightConstraint);
        }
    }
}
