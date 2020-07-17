using TodoList.Pages;
using TodoList.Storage.Database.Implementation;
using Xamarin.Forms;

namespace TodoList
{
    public partial class App : Application
    {
        private static TodoDatabase database;
        public static TodoDatabase Database
        {
            get
            {
                if (database is null)
                {
                    database = new TodoDatabase();
                }

                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            Pages.MainPage.Instance.TryCheckAndUpdateNotifications();
        }

        protected override void OnSleep()
        {
            Pages.MainPage.Instance.TryCheckAndUpdateNotifications();
        }

        protected override void OnResume()
        {
            Pages.MainPage.Instance.TryCheckAndUpdateNotifications();
        }
    }
}
