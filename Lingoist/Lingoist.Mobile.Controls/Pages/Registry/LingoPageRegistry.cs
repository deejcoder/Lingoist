using Lingoist.Mobile.UI.Pages.Abstraction;
using Lingoist.Mobile.UI.Pages.Exceptions;
using Lingoist.Mobile.UI.Pages.Models;

namespace Lingoist.Mobile.UI.Pages.Registry
{
    public class LingoPageRegistry
    {
        private static Dictionary<string, LingoPageDescriptor> _pages = [];

        public static IEnumerable<LingoPageDescriptor> Pages => _pages.Values.AsEnumerable();

        internal static void Register<TPage>(LingoPageOptions options) where TPage : class, ILingoPage
        {
            LingoPageDescriptor descriptor = new(typeof(TPage), options.Recycle);

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

        internal static LingoPageDescriptor GetPageDescriptor(Type pageType)
        {
            // get the page from the registry 
            LingoPageDescriptor? descriptor = Pages.FirstOrDefault(p => p.Type == pageType);
            if (descriptor == null)
            {
                throw new LingoPageNotRegisteredException(pageType?.FullName ?? "Unknown");
            }
            return descriptor;
        }
    }
}
