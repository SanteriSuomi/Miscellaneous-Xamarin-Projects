using TodoList.Pages;
using TodoList.Storage.Database.Implementation;
using Xamarin.Forms;

namespace TodoList
{
    public partial class App : Application
    {
        private static TodoDatabase todoDatabase;
        public static TodoDatabase TodoDatabase
        {
            get
            {
                if (todoDatabase is null)
                {
                    todoDatabase = new TodoDatabase();
                }

                return todoDatabase;
            }
        }

        public static readonly string BackgroundColorKey = "BackgroundColor";
        public static readonly string BarColorKey = "BarColor";
        public static readonly string[] PageTextColorKeys = new string[] { "NavigationTitleTextColor", "CreateEditPageEntryEditorColor" };
        public static readonly string[] ButtonTextColorKeys = new string[] { "NavigationButtonTextColor", "CreateEditPageButtonTextColor" };
        public static readonly string TodoListColorKey = "MainPageCollectionItemColor";

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            LoadThemeSettings();
            Pages.MainPage.Instance?.TryCheckAndUpdateNotifications();
        }

        private static void LoadThemeSettings()
        {
            var fields = Current.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.Name.ToLowerInvariant().Contains("color"))
                {
                    if (field.FieldType == typeof(string[]))
                    {
                        var fieldArray = (string[])field.GetValue(null);
                        foreach (var fieldValue in fieldArray)
                        {
                            TrySetProperty(fieldValue);
                        }
                    }
                    else
                    {
                        var fieldString = (string)field.GetValue(null);
                        TrySetProperty(fieldString);
                    }
                }
            }
        }

        private static void TrySetProperty(string key)
        {
            if (Current.Properties.ContainsKey(key))
            {
                Current.Resources[key] = Color.FromHex((string)Current.Properties[key]);
            }
        }

        protected override void OnSleep()
        {
            Pages.MainPage.Instance?.TryCheckAndUpdateNotifications();
        }

        protected override void OnResume()
        {
            Pages.MainPage.Instance?.TryCheckAndUpdateNotifications();
        }
    }
}
