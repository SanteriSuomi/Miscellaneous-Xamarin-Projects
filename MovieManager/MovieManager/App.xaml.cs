using Xamarin.Forms;

namespace MovieManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.DarkRed,
                BarTextColor = Color.FromHex("#292929")
            };
        }

        protected override void OnStart()
        {
            // Method intentionally left empty.
        }

        protected override void OnSleep()
        {
            // Method intentionally left empty.
        }

        protected override void OnResume()
        {
            // Method intentionally left empty.
        }
    }
}