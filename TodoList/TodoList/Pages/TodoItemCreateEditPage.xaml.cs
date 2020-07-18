using Plugin.FilePicker;
using Plugin.LocalNotifications;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Extensions;
using TodoList.Storage.ConfigSettings;
using TodoList.Utilities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    public enum CreateEditPageType
    {
        Create,
        Edit
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoItemCreateEditPage : ContentPage
    {
        private readonly CreateEditPageType pageType;
        private readonly string originalEntryTitlePlaceHolderText;
        private readonly string originalEditorBodyPlaceHolderText;

        private readonly ObservableCollection<TodoItem> mainTodoCollection;
        private readonly TodoItem localTodo;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown", Justification = "Dev")]
        public TodoItemCreateEditPage(ObservableCollection<TodoItem> todoCollection, TodoItem editingItem, CreateEditPageType pageType, string pageTitlePrefix)
        {
            InitializeComponent();
            this.mainTodoCollection = todoCollection;
            originalEntryTitlePlaceHolderText = entryTitle.Placeholder;

            // Because xamarin editor does not have a placeholder horizontal alignment property, attempt to center the text manually by using the device's width.
            StringBuilder editorPlaceHolderBuilder = new StringBuilder();
            int amountOfWhiteSpace = (int)(DeviceDisplay.MainDisplayInfo.Width / 100) + 3;
            for (int i = 0; i < amountOfWhiteSpace; i++)
            {
                editorPlaceHolderBuilder.Append(' ');
            }
            editorPlaceHolderBuilder.Append("Body");
            originalEditorBodyPlaceHolderText = editorPlaceHolderBuilder.ToString();
            editorBody.Placeholder = originalEditorBodyPlaceHolderText;

            titleLabel.Text = $"{pageTitlePrefix} To-Do";
            this.pageType = pageType;
            if (this.pageType == CreateEditPageType.Edit)
            {
                localTodo = editingItem;
                entryTitle.Text = localTodo.Title;
                editorBody.Text = localTodo.Body;
                chooseImageLabel.Text = localTodo.ImageName;

                var toggled = localTodo.HasNotification;
                notificationSwitch.IsToggled = toggled;

                var (hasData, dateAndTime) = localTodo.GetCombinedDateTime(false);
                if (localTodo.HasNotification
                    && hasData)
                {
                    datePicker.Date = dateAndTime.date;
                    timePicker.Time = dateAndTime.time;
                }
            }
            else
            {
                localTodo = new TodoItem()
                {
                    Date = DateTime.Now.ToString()
                };
            }
        }

        #region Back
        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }
        #endregion

        #region Save
        private async void OnSaveButtonPressed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryTitle.Text)
                || entryTitle.Text.Length == 0

                || string.IsNullOrWhiteSpace(editorBody.Text)
                || editorBody.Text.Length == 0)
            {
                await DisplayAlert("Error", "Title or body is empty", "Cancel");
                return;
            }
            else if (notificationSwitch.IsToggled
                     && !IsSelectedDateAndTimeValid(timePicker))
            {
                await DisplayAlert("Error", "Notification date or time is not valid", "Cancel");
                return;
            }

            TrySetDefaultImage();
            TrimTodoText();
            TrySetNotification();
            await SaveOrUpdateCollectionAndDatabase();
            await Navigation.PopToRootAsync();
        }

        private void TrySetNotification()
        {
            var (hasData, dateAndTime) = localTodo.GetCombinedDateTime(true);
            if (hasData && localTodo.HasNotification)
            {
                var truncatedBody = $"{localTodo.Body.Truncate(Config.ST.NotificationBodyLength)}...";
                var notificationId = RandomUtilities.GetRandomInt(1, Config.ST.NotificationMaxRandom);
                var exactDateAndTime = dateAndTime.date + dateAndTime.time;
                CrossLocalNotifications.Current.Show(localTodo.Title, truncatedBody, notificationId, exactDateAndTime);
                localTodo.NotificationId = notificationId;
            }
            else if (!localTodo.HasNotification)
            {
                CrossLocalNotifications.Current.Cancel(localTodo.NotificationId);
            }
        }

        private void TrySetDefaultImage()
        {
            if (!localTodo.HasImage)
            {
                localTodo.Image = Config.ST.DefaultTodoIconPath;
            }
        }

        private void TrimTodoText()
        {
            localTodo.Title = localTodo.Title.Trim();
            localTodo.Body = localTodo.Body.Trim();
        }

        private async Task SaveOrUpdateCollectionAndDatabase()
        {
            if (pageType == CreateEditPageType.Create)
            {
                mainTodoCollection.Add(localTodo);
                await App.TodoDatabase.Save(localTodo);
                await DisplayAlert("New To-Do Saved", "Item has been saved", "Ok");
            }
            else
            {
                await App.TodoDatabase.Update(localTodo);
                await DisplayAlert("Edit To-Do Saved", "Item has been saved", "Ok");
            }
        }
        #endregion

        #region Title
        private void OnEntryTitleFocused(object sender, FocusEventArgs e)
        {
            entryTitle.Placeholder = string.Empty;
        }

        private void OnEntryTitleUnfocused(object sender, FocusEventArgs e)
        {
            entryTitle.Placeholder = originalEntryTitlePlaceHolderText;
        }

        private void OnEntryTitleCompleted(object sender, EventArgs e)
        {
            localTodo.Title = entryTitle.Text;
        }

        private void OnEntryTitleTextChanged(object sender, TextChangedEventArgs e)
        {
            localTodo.Title = entryTitle.Text;
        }
        #endregion

        #region Body
        private void OnEditorBodyFocused(object sender, FocusEventArgs e)
        {
            // For some reason, keyboard does not show when setting placeholder to empty unless forced to be ran on the main thread.
            Device.BeginInvokeOnMainThread(() => editorBody.Placeholder = string.Empty);
        }

        private void OnEditorBodyUnfocused(object sender, FocusEventArgs e)
        {
            editorBody.Placeholder = originalEditorBodyPlaceHolderText;
        }

        private void OnEditorBodyCompleted(object sender, EventArgs e)
        {
            localTodo.Body = editorBody.Text;
        }

        private void OnEditorBodyTextChanged(object sender, TextChangedEventArgs e)
        {
            localTodo.Body = editorBody.Text;
        }
        #endregion

        #region Image
        private async void OnChooseImageButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile(Config.ST.AllowedTodoImageTypes);
                if (file == null) return;

                chooseImageLabel.Text = file.FileName;
                localTodo.Image = file.FilePath;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            chooseImageButton.Text = "Choose New Image";
        }

        private async void OnImageLabelButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (string.IsNullOrEmpty(button.Text)) return;

            await Navigation.PushAsync(new TodoItemImageViewPage(localTodo.Image, chooseImageLabel.Text), true);
        }
        #endregion

        #region Date/Time
        private void OnNotificationSwitchToggled(object sender, ToggledEventArgs e)
        {
            SetNotificationState(e.Value);
        }

        private void SetNotificationState(bool value)
        {
            datePicker.IsEnabled = value;
            timePicker.IsEnabled = value;

            if (value)
            {
                localTodo.HasNotification = true;
                datePickerLabel.Text = "Date";
                timePickerLabel.Text = "Time";
            }
            else
            {
                localTodo.HasNotification = false;
                datePickerLabel.Text = string.Empty;
                timePickerLabel.Text = string.Empty;
            }
        }

        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            var dateOffset = DateTime.Compare(picker.Date, DateTime.Today);
            if (dateOffset < 0)
            {
                await DisplayAlert("Date Error", "Please choose a date that's today or in the future", "Cancel");
                return;
            }

            localTodo.Date = picker.Date.ToString();
        }

        private async void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var picker = sender as TimePicker;
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                if (!IsSelectedDateAndTimeValid(picker))
                {
                    await DisplayAlert("Time Error", "Since notification is set for today, please choose a time in the future", "Cancel");
                    return;
                }

                localTodo.Time = picker.Time.ToString();
            }
        }

        private bool IsSelectedDateAndTimeValid(TimePicker picker)
        {
            return !(datePicker.Date == DateTime.Today
                    && picker.Time.TotalMinutes <= DateTime.Now.TimeOfDay.TotalMinutes);
        }
        #endregion


    }
}