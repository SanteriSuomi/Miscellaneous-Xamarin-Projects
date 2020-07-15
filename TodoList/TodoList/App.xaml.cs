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
