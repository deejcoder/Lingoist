namespace Lingoist.Mobile.UI.Pages.Registry
{
    public class LingoPageOptions
    {
        /// <summary>
        /// If enabled, this page does not support backwards navigation.
        /// When navigating forward, previous pages of the same type are recycled.
        /// </summary>
        public bool Recycle { get; set; } = false;
    }
}
