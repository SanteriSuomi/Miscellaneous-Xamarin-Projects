using System.ComponentModel;
using TodoList.Storage.Config;
using Xamarin.Forms;

namespace TodoList
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            testLabel.Text = Config.Settings.DatabaseName;
        }
    }
}
