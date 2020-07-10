using Xamarin.Forms;

namespace PhotoGallery
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            MainPage = new MainPage();
        }
    }
}
