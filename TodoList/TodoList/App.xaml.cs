using TodoList.Pages;
using TodoList.Storage.Database.Implementation;
using Xamarin.Forms;

namespace TodoList
{
    public partial class App : Application
    {
        public static TodoDatabase Database { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            Database = new TodoDatabase();
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
