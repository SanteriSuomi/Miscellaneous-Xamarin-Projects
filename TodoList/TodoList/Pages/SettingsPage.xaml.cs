using Amporis.Xamarin.Forms.ColorPicker;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void OnBackgroundColorPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await SetProperties(e, sender, App.BackgroundColorKey);
        }

        private async void OnBarColorPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await SetProperties(e, sender, App.BarColorKey);
        }

        private async void OnPageColorPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            for (int i = 0; i < App.PageTextColorKeys.Length; i++)
            {
                await SetProperties(e, sender, App.PageTextColorKeys[i]);
            }
        }

        private async void OnButtonColorPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            for (int i = 0; i < App.ButtonTextColorKeys.Length; i++)
            {
                await SetProperties(e, sender, App.ButtonTextColorKeys[i]);
            }
        }

        private async void OnTodoListItemPickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await SetProperties(e, sender, App.TodoListColorKey);
        }

        private async static Task SetProperties(PropertyChangedEventArgs args, object sender, string key)
        {
            if (args.PropertyName == "Color")
            {
                var color = (sender as ColorPickerEntry).Color;
                Application.Current.Resources[key] = color;
                Application.Current.Properties[key] = color.ToHex();
                await Application.Current.SavePropertiesAsync();
            }
        }
    }
}