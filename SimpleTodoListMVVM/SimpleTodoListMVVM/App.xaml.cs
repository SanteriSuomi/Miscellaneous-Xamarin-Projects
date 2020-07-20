using Xamarin.Forms;

namespace SimpleTodoListMVVM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPageView());
        }

        protected override void OnStart()
        {
            // Not implemented on purpose.
        }

        protected override void OnSleep()
        {
            // Not implemented on purpose.
        }

        protected override void OnResume()
        {
            // Not implemented on purpose.
        }
    }
}
