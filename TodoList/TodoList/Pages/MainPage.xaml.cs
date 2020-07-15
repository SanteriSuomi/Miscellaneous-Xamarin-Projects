using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static MainPage Instance { get; private set; }

        private const double multiselectBarTranslationAmount = 100;
        private const uint multiselectBarTranslationDuration = 750;
        private static readonly Easing multiselectBarEasing = Easing.SinOut;

        private ObservableCollection<TodoItem> todoCollection;

        private readonly List<object> emptyList = new List<object>();
        private bool multiselectBarIsVisible;

        public MainPage()
        {
            Instance = this;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var todoItems = await App.Database.GetAll();
            collectionView.ItemsSource = todoCollection = new ObservableCollection<TodoItem>(todoItems);
        }

        private void OnNewTodoButtonPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewTodoItemPage(todoCollection), true);
        }

        private async void OnSingleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collectionView.SelectionMode != SelectionMode.Single) return;


        }

        public async Task OnSelectionLongPressed(TodoItem pressedItem)
        {
            if (multiselectBarIsVisible) return;

            multiselectBarIsVisible = true;
            collectionView.SelectionMode = SelectionMode.Multiple;
            collectionView.SelectedItems.Add(pressedItem);
            await TranslateMultiselectBar(multiselectBarTranslationAmount);
        }

        private async void OnMultiselectBarCancelButtonPressed(object sender, EventArgs e)
        {
            await CancelMultiselectBar();
        }

        private async Task CancelMultiselectBar()
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

        private async void OnMultiselectBarDeleteButtonPressed(object sender, EventArgs e)
        {
            var selectedItems = collectionView.SelectedItems;
            for (int i = 0; i < selectedItems.Count; i++)
            {
                var todo = selectedItems[i] as TodoItem;
                todoCollection.Remove(todo);
                await App.Database.Remove(todo);
            }

            await CancelMultiselectBar();
        }
    }
}