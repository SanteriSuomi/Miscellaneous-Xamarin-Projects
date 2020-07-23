using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesBrowser.Modules.MovieInfoPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieInfoView : ContentPage
    {
        public MovieInfoView(MovieInfoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}