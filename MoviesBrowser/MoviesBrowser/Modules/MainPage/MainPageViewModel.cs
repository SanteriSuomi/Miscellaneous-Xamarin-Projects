using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MoviesBrowser.Modules.MainPage
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel()
        {
            Title = "Title";
            OnButtonClick = new Command(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                Title = "Title2";
            });
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public ICommand OnButtonClick { get; set; }
    }
}