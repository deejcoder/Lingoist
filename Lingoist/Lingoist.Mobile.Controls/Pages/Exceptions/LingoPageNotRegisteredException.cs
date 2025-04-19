namespace Lingoist.Mobile.UI.Pages.Exceptions
{
    public class LingoPageNotRegisteredException : Exception
    {
        public LingoPageNotRegisteredException(string pageName) : base($"Page {pageName} is not registered.")
        {
        }
        public LingoPageNotRegisteredException(string pageName, Exception innerException) : base($"Page {pageName} is not registered.", innerException)
        {
        }
    }
}
