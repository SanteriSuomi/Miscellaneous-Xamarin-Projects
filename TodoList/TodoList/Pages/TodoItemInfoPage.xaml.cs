using TodoList.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.ObjectModel;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoItemInfoPage : ContentPage
    {
        public TodoItem TodoItem { get; set; }
        private readonly ObservableCollection<TodoItem> todoCollection;

        public TodoItemInfoPage(TodoItem item, ObservableCollection<TodoItem> todoCollection)
        {
            InitializeComponent();
            TodoItem = item;
            this.todoCollection = todoCollection;
            BindingContext = TodoItem;
        }

        private async void OnImageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoItemImageViewPage(TodoItem.Image, TodoItem.ImageName), true);
        }

        private async void OnToolbarEditButtonPressed(object sender, EventArgs e)
        {
            bool result = await DisplayAlert($"Edit {TodoItem.Title}", $"Edit To-Do {TodoItem.Title}?", "Accept", "Cancel");
            if (result)
            {
                await Navigation.PushAsync(new TodoItemCreateEditPage(todoCollection, TodoItem, CreateEditPageType.Edit, "Edit"), true);
            }
        }
    }
}