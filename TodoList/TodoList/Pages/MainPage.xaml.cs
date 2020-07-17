using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Storage.ConfigSettings;
using Xamarin.Forms;

namespace TodoList.Pages
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static MainPage Instance { get; private set; }

        private ObservableCollection<TodoItem> todoCollection;

        private readonly Easing multiselectBarEasing;
        private readonly List<object> emptyCollection = new List<object>();
        private bool multiselectBarIsVisible;

        public MainPage()
        {
            Instance = this;
            InitializeComponent();
            multiselectBarEasing = Config.GetFieldValue<Easing>(Config.ST.MultiselectBarEasingType);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (todoCollection is null)
            {
                var todoItems = await App.Database.GetAll();
                collectionView.ItemsSource = todoCollection = new ObservableCollection<TodoItem>(todoItems);
            }
        }

        /// <summary>
        /// Check every saved item for notification changes, and update them accordingly.
        /// </summary>
        public void TryCheckAndUpdateNotifications()
        {
            if (todoCollection is null) return;

            foreach (var item in todoCollection)
            {
                var (hasData, dateAndTime) = item.GetCombinedDateTime(true);
                var totalTime = dateAndTime.date + dateAndTime.time;
                if (hasData
                    && totalTime < DateTime.Now)
                {
                    CrossLocalNotifications.Current.Cancel(item.NotificationId);
                    item.HasNotification = false;
                    item.NotificationId = default;
                }
            }
        }

        private void OnNewTodoButtonPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TodoItemCreateEditPage(todoCollection, null, CreateEditPageType.Create, "New"), true);
        }

        private async void OnSingleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (collectionView.SelectionMode != SelectionMode.Single
                || e.CurrentSelection.Count == 0) return;

            var selection = e.CurrentSelection[0] as TodoItem;
            await Navigation.PushAsync(new TodoItemInfoPage(selection, todoCollection), true);
        }

        public async Task OnSelectionLongPressed(TodoItem pressedItem)
        {
            if (multiselectBarIsVisible) return;

            multiselectBarIsVisible = true;
            collectionView.SelectionMode = SelectionMode.Multiple;
            collectionView.SelectedItems.Add(pressedItem);
            await TranslateMultiselectBar(Config.ST.MultiselectBarTranslationAmount);
        }

        private async void OnMultiselectBarCancelButtonPressed(object sender, EventArgs e)
        {
            await CancelMultiselectBar();
        }

        private async Task TranslateMultiselectBar(double yTranslation)
        {
            double x = multiSelectionBar.TranslationX;
            double y = multiSelectionBar.TranslationY + yTranslation;
            await multiSelectionBar.TranslateTo(x, y, Config.ST.MultiselectBarTranslationDuration, multiselectBarEasing);
        }

        private async void OnMultiselectBarDeleteButtonPressed(object sender, EventArgs e)
        {
            await RemoveSelectedItems();
            await CancelMultiselectBar();
        }

        private async Task RemoveSelectedItems()
        {
            var selectedItems = collectionView.SelectedItems;
            for (int i = 0; i < selectedItems.Count; i++)
            {
                var todo = selectedItems[i] as TodoItem;
                CrossLocalNotifications.Current.Cancel(todo.NotificationId);
                todoCollection.Remove(todo);
                await App.Database.Remove(todo);
            }
        }

        private async Task CancelMultiselectBar()
        {
            multiselectBarIsVisible = false;
            collectionView.SelectionMode = SelectionMode.Single;
            collectionView.SelectedItems = emptyCollection;
            await TranslateMultiselectBar(-Config.ST.MultiselectBarTranslationAmount);
        }

        private async void OnSettingsButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}