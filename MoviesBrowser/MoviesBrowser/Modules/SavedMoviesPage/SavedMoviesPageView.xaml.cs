using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesBrowser.Modules.SavedMoviesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedMoviesPageView : ContentPage
    {
        public SavedMoviesPageView(SavedMoviesPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}