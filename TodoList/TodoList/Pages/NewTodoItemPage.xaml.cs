using Plugin.FilePicker;
using System;
using TodoList.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTodoItemPage : ContentPage
    {
        private static readonly string[] filePickerAllowedTypes = new string[] { "image/*" };

        private readonly CollectionView mainCollectionView;
        private readonly string originalEntryTitlePlaceHolderText;
        private readonly string originalEditorBodyPlaceHolderText;

        private readonly TodoItem newTodoItem;

        public NewTodoItemPage(CollectionView mainCollectionView)
        {
            InitializeComponent();
            this.mainCollectionView = mainCollectionView;
            originalEntryTitlePlaceHolderText = entryTitle.Placeholder;
            originalEditorBodyPlaceHolderText = editorBody.Placeholder;
            newTodoItem = new TodoItem();
        }

        private void OnSaveButtonPressed(object sender, EventArgs e)
        {

        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private void OnEntryTitleFocused(object sender, FocusEventArgs e)
        {
            entryTitle.Placeholder = string.Empty;
        }

        private void OnEntryTitleUnfocused(object sender, FocusEventArgs e)
        {
            entryTitle.Placeholder = originalEntryTitlePlaceHolderText;
        }

        private void OnEditorBodyFocused(object sender, FocusEventArgs e)
        {
            // For some reason, keyboard does not show when setting placeholder to empty unless forced to be ran on the main thread.
            Device.BeginInvokeOnMainThread(() => editorBody.Placeholder = string.Empty);
        }

        private void OnEditorBodyUnfocused(object sender, FocusEventArgs e)
        {
            editorBody.Placeholder = originalEditorBodyPlaceHolderText;
        }

        private async void OnChooseImageButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var file = await CrossFilePicker.Current.PickFile(filePickerAllowedTypes);
                if (file == null) return;

                chooseImageLabel.Text = file.FileName;
                newTodoItem.Image = file.FilePath;
                //chooseImage.Source = ImageSource.FromStream(() => file.GetStream());
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            //var pickerService = DependencyService.Get<IImagePickerService>();
            //var data = await pickerService.GetImageDataAsync();
            //if (data != null)
            //{
            //    //chooseImage.Source = ImageSource.FromStream(() => data.Stream);
            //    chooseImageLabel.Text = data.FileName;
            //}
        }
    }
}