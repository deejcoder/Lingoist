using Lingoist.Mobile.UI.Pages;

namespace Lingoist.Mobile.Pages
{
    public class LingoistPageHost : LingoistPageHostController
    {
        public override Task<ILingoistPage> CreatePage<TPage>(bool animated = true)
        {
            if(typeof(TPage) == typeof(AddEditPracticeSetPage))
            {
                return Task.FromResult<ILingoistPage>(new AddEditPracticeSetPage());
            }
            else
            {
                throw new NotImplementedException($"The page {typeof(TPage).Name} is not implemented.");
            }
        }
    }
}
