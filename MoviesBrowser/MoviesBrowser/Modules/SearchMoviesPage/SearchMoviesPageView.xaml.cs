using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesBrowser.Modules.SearchMoviesPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMoviesPageView : ContentPage
    {
        public SearchMoviesPageView(SearchMoviesPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}