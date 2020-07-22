using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoviesBrowser.Modules
{
    public abstract class BaseViewModel : BindableObject
    {
        public virtual Task InitializeAsync(object parameter)
        {
            return Task.CompletedTask;
        }
    }
}