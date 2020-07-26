using MoviesBrowser.Modules;
using System.Threading.Tasks;

namespace MoviesBrowser.Common.Navigation
{
    public interface INavigationService
    {
        Task<bool> PushAsync<TViewModel>(object parameter = null, bool animated = true) where TViewModel : BaseViewModel;
        Task PopAsync();
    }
}