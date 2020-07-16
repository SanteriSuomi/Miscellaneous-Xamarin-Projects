using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoItemImageViewPage : ContentPage
    {
        public string ImagePath { get; }
        public string ImageName { get; }

        public TodoItemImageViewPage(string imagePath, string imageName)
        {
            InitializeComponent();
            ImagePath = imagePath;
            ImageName = imageName;
            BindingContext = this;
        }
    }
}