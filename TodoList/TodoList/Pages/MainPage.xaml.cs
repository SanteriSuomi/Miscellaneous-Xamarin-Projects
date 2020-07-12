using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TodoList.Data;
using Xamarin.Forms;

namespace TodoList.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ICommand LongPressCommand { get; }

        public MainPage()
        {
            LongPressCommand = new Command(OnSelectionLongPressed);
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = new List<TodoItem>()
            {
                new TodoItem
                {
                    Title = "Lorem ipsum Lorem ipsum Lorem ipsum Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                },
                new TodoItem
                {
                    Title = "Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                },
                new TodoItem
                {
                    Title = "Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                },
                new TodoItem
                {
                    Title = "Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                },
                new TodoItem
                {
                    Title = "Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                },
                new TodoItem
                {
                    Title = "Lorem ipsum",
                    Image = ImageSource.FromUri(new Uri("https://i.picsum.photos/id/149/200/200.jpg?hmac=ykhZe9T_HysK0voTz01NVBW7C8XlLYYT2EinqAhTA-0"))
                }
            };
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private async void OnSelectionLongPressed()
        {
            Console.Write("LONGPRESSED:");
            await DisplayAlert("Long Pressed", "Long Pressed", "Cancel");
        }
    }
}