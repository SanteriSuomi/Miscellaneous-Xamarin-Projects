using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TodoList.Data;
using Xamarin.Forms;

namespace TodoList.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static MainPage This { get; private set; }

        private const double multiselectBarTranslationAmount = 100;
        private const uint multiselectBarTranslationDuration = 750;
        private static readonly Easing multiselectBarEasing = Easing.SinOut;

        private readonly List<object> emptyList = new List<object>();
        private bool multiselectBarIsVisible;

        public MainPage()
        {
            InitializeComponent();
            This = this;
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

        private void OnNewTodoButtonPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewTodoItemPage(collectionView), true);
        }

        private async void OnSingleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collectionView.SelectionMode != SelectionMode.Single) return;

            //await DisplayAlert("Title", "Title", "Title");
        }

        public async Task OnSelectionLongPressed()
        {
            if (multiselectBarIsVisible) return;

            multiselectBarIsVisible = true;
            await TranslateMultiselectBar(multiselectBarTranslationAmount);
            collectionView.SelectionMode = SelectionMode.Multiple;
        }

        private async void OnMultiselectBarButtonPressed(object sender, EventArgs e)
        {
            multiselectBarIsVisible = false;
            collectionView.SelectedItems = emptyList;
            await TranslateMultiselectBar(-multiselectBarTranslationAmount);
            collectionView.SelectionMode = SelectionMode.Single;
        }

        private async Task TranslateMultiselectBar(double yTranslation)
        {
            double x = multiSelectionBar.TranslationX;
            double y = multiSelectionBar.TranslationY + yTranslation;
            await multiSelectionBar.TranslateTo(x, y, multiselectBarTranslationDuration, multiselectBarEasing);
        }
    }
}