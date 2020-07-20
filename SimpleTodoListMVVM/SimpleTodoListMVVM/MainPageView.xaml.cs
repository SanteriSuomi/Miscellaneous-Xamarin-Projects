using SimpleTodoListMVVM.Extensions;
using SimpleTodoListMVVM.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleTodoListMVVM
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageView : ContentPage
    {
        public MainPageView()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            MessagingCenter.Subscribe<MainPageViewModel, MsgArgs<ObservableCollection<TodoItem>>>(this, "OnNewItem", (sender, message) =>
            {
                OnNewItem(message).SafeFireAndForget();
            });
            MessagingCenter.Subscribe<MainPageViewModel, MsgArgs<TodoItem, ObservableCollection<TodoItem>>>(this, "OnDelete", (sender, message) =>
            {
                OnDelete(message).SafeFireAndForget();
            });
        }

        private async Task OnNewItem(MsgArgs<ObservableCollection<TodoItem>> message)
        {
            await DisplayAlert("New Item", "Add New Item", "Accept", "Cancel");
            message.Object.Add(new TodoItem()
            {
                Title = "XDDDD"
            });
        }

        private async Task OnDelete(MsgArgs<TodoItem, ObservableCollection<TodoItem>> message)
        {
            var result = await DisplayAlert("Remove", $"Remove '{message.Object.Title}'?", "Accept", "Cancel");
            if (result)
            {
                message.Object2.Remove(message.Object);
            }
        }
    }
}