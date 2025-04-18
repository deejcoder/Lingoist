using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Models;

namespace Lingoist.Mobile.UI.Pages.Registry
{
    public class LingoistPageRegistry
    {
        private static Dictionary<string, LingoistPageDescriptor> _pages = [];

        public static IEnumerable<LingoistPageDescriptor> Pages => _pages.Values.AsEnumerable();

        public static void Register<TPage>() where TPage : class, ILingoPage
        {
            LingoistPageDescriptor descriptor = new(typeof(TPage));

            string fullName = descriptor.Type?.FullName ?? string.Empty;

            if(!string.IsNullOrEmpty(fullName))
            {
                if (_pages.ContainsKey(fullName))
                {
                    throw new Exception($"Page {fullName} is already registered.");
                }
                _pages.Add(fullName, descriptor);
            }
            else
            {
                throw new Exception($"Page {descriptor.Type!.Name} does not have a full name.");
            }            
        }        
    }
}
