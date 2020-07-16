using Plugin.FilePicker;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using TodoList.Data;
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

        private static readonly string[] filePickerAllowedTypes = new string[] { "image/*" };

        private readonly string originalEntryTitlePlaceHolderText;
        private readonly string originalEditorBodyPlaceHolderText;

        private readonly ObservableCollection<TodoItem> todoCollection;
        private readonly TodoItem localTodo;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown", Justification = "Dev")]
        public TodoItemCreateEditPage(ObservableCollection<TodoItem> todoCollection, TodoItem editingItem, CreateEditPageType pageType, string pageTitlePrefix)
        {
            InitializeComponent();
            this.todoCollection = todoCollection;
            originalEntryTitlePlaceHolderText = entryTitle.Placeholder;
            originalEditorBodyPlaceHolderText = editorBody.Placeholder;


            Title = $"{pageTitlePrefix} To-Do";
            this.pageType = pageType;
            if (this.pageType == CreateEditPageType.Edit)
            {
                localTodo = editingItem;
                entryTitle.Text = localTodo.Title;
                editorBody.Text = localTodo.Body;
                chooseImageLabel.Text = localTodo.ImageName;
                if (DateTime.TryParse(localTodo.Date, out DateTime dateResult)
                    && TimeSpan.TryParse(localTodo.Time, out TimeSpan timeResult))
                {
                    datePicker.Date = dateResult;
                    timePicker.Time = timeResult;
                }
                else
                {
                    throw new NullReferenceException("Could not parse Date or Time of localTodo item");
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

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

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
            else if (!IsSelectedDateAndTimeValid(timePicker))
            {
                await DisplayAlert("Error", "Notification date or time is not valid", "Cancel");
                return;
            }

            TrimTodoText();
            await SaveOrUpdateItem();
            await Navigation.PopAsync();
        }

        private async Task SaveOrUpdateItem()
        {
            if (pageType == CreateEditPageType.Create)
            {
                todoCollection.Add(localTodo);
                await App.Database.Save(localTodo);
                await DisplayAlert("New To-Do Saved", "Item has been saved", "Ok");
            }
            else
            {
                await App.Database.Update(localTodo);
                await DisplayAlert("Edit To-Do Saved", "Item has been saved", "Ok");
            }
        }

        private void TrimTodoText()
        {
            localTodo.Title = localTodo.Title.Trim();
            localTodo.Body = localTodo.Body.Trim();
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
                var file = await CrossFilePicker.Current.PickFile(filePickerAllowedTypes);
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
            SetNotificationPickerVisibilities(e.Value);
        }

        private void SetNotificationPickerVisibilities(bool value)
        {
            datePicker.IsEnabled = value;
            timePicker.IsEnabled = value;

            if (value)
            {
                datePickerLabel.Text = "Date";
                timePickerLabel.Text = "Time";
            }
            else
            {
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