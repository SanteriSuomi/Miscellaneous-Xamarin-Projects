using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TodoList.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTodoItemPage : ContentPage
    {
        private readonly CollectionView mainCollectionView;
        private readonly string originalEntryTitlePlaceHolderText;
        private readonly string originalEditorBodyPlaceHolderText;

        public NewTodoItemPage(CollectionView mainCollectionView)
        {
            InitializeComponent();
            this.mainCollectionView = mainCollectionView;
            originalEntryTitlePlaceHolderText = entryTitle.Placeholder;
            originalEditorBodyPlaceHolderText = editorBody.Placeholder;
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
    }
}