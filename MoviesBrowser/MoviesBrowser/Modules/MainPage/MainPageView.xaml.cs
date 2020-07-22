using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesBrowser.Modules.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageView : ContentPage
    {
        public MainPageView(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}