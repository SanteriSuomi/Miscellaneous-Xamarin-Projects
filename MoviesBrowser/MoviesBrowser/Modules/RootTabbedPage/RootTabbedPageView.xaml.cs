using Autofac;
using MoviesBrowser.Modules.SearchMoviesPage;
using MoviesBrowser.Modules.SavedMoviesPage;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesBrowser.Modules.RootTabbedPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootTabbedPageView : TabbedPage
    {
        public RootTabbedPageView(RootTabbedPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            Children.Add(App.Container.Resolve<SearchMoviesPageView>());
            Children.Add(App.Container.Resolve<SavedMoviesPageView>());
        }
    }
}