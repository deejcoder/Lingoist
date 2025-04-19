namespace Lingoist.Mobile.UI.Pages.Models
{
    public sealed class LingoPageDescriptor
    {
        public Type Type { get; }
        public bool Recycle { get; }

        internal LingoPageDescriptor(Type type, bool recycle)
        {
            Type = type;
            Recycle = recycle;
        }
    }
}
