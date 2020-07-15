using Plugin.FilePicker;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TodoList.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTodoItemPage : ContentPage
    {
        private static readonly string[] filePickerAllowedTypes = new string[] { "image/*" };

        private readonly string originalEntryTitlePlaceHolderText;
        private readonly string originalEditorBodyPlaceHolderText;

        private readonly ObservableCollection<TodoItem> todoCollection;
        private readonly TodoItem localNewTodoItem;

        public NewTodoItemPage(ObservableCollection<TodoItem> todoCollection)
        {
            InitializeComponent();
            this.todoCollection = todoCollection;
            originalEntryTitlePlaceHolderText = entryTitle.Placeholder;
            originalEditorBodyPlaceHolderText = editorBody.Placeholder;
            localNewTodoItem = new TodoItem()
            {
                Date = DateTime.Now.ToString()
            };
        }

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

            TrimTodoText();
            todoCollection.Add(localNewTodoItem);
            await App.Database.Save(localNewTodoItem);
            await DisplayAlert("Saved", "Item has been saved", "Ok");
            await Navigation.PopAsync();
        }

        private void TrimTodoText()
        {
            localNewTodoItem.Title = localNewTodoItem.Title.Trim();
            localNewTodoItem.Body = localNewTodoItem.Body.Trim();
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

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
            localNewTodoItem.Title = entryTitle.Text;
        }

        private void OnEntryTitleTextChanged(object sender, TextChangedEventArgs e)
        {
            localNewTodoItem.Title = entryTitle.Text;
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
            localNewTodoItem.Body = editorBody.Text;
        }

        private void OnEditorBodyTextChanged(object sender, TextChangedEventArgs e)
        {
            localNewTodoItem.Body = editorBody.Text;
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
                localNewTodoItem.Image = file.FilePath;
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

            await Navigation.PushAsync(new NewTodoItemImagePreviewPage(localNewTodoItem.Image, chooseImageLabel.Text), true);
        }
        #endregion

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            var datePicker = sender as DatePicker;
            localNewTodoItem.Date = datePicker.Date.ToString();
        }

        private void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var timePicker = sender as TimePicker;
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                localNewTodoItem.Time = timePicker.Time.ToString();
            }
        }
    }
}