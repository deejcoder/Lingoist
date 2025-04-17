using CommunityToolkit.Mvvm.ComponentModel;

namespace Lingoist.Mobile.UI.Pages
{
    public abstract partial class LingoistPageHostController : ObservableObject
    {
        [ObservableProperty]
        public partial ILingoistPage? Current { get; set; }

        public abstract Task<ILingoistPage> CreatePage<TPage>(bool animated = true) where TPage : ILingoistPage;        

        public async virtual Task NavigateTo<TPage>(object? state) where TPage : ILingoistPage
        {
            ILingoistPage page = await CreatePage<TPage>();            
            this.Current = page;
        }
    }
}
