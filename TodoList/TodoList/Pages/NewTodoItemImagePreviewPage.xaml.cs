using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTodoItemImagePreviewPage : ContentPage
    {
        public string ImagePath { get; }
        public string ImageName { get; }

        public NewTodoItemImagePreviewPage(string imagePath, string imageName)
        {
            InitializeComponent();
            ImagePath = imagePath;
            ImageName = imageName;
            BindingContext = this;
        }
    }
}