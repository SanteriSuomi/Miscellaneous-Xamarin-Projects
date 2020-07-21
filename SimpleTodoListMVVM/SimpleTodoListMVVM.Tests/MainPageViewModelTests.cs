using SimpleTodoListMVVM.Utilities;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xunit;

namespace SimpleTodoListMVVM.Tests
{
    public class MainPageViewModelTests
    {
        [Fact]
        public void ViewModel_items_get_populated_and_initialized_in_construction_correctly()
        {
            var viewModel = new MainPageViewModel();
            Assert.NotNull(viewModel.Items);
        }

        [Fact]
        public void OnNewItem_populates_list_with_new_item()
        {
            var viewModel = new MainPageViewModel();
            MessagingCenter.Subscribe<MainPageViewModel, MsgArgs<ObservableCollection<TodoItem>>>(this, "OnNewItem", (sender, message) =>
            {
                message.Object.Add(new TodoItem());
            });

            var oldCount = viewModel.Items.Count;
            viewModel.OnNewItemCommand.Execute(null);
            var newCount = viewModel.Items.Count;
            Assert.Equal(oldCount + 1, newCount);
        }

        [Fact]
        public void OnComplete_completes_item_and_moves_item_to_last_index()
        {
            var viewModel = new MainPageViewModel();
            var item = new TodoItem();
            viewModel.Items.Add(item);
            viewModel.OnCompleteCommand.Execute(item);
            Assert.True(item.IsComplete);
            Assert.Equal(viewModel.Items[^1], item);
        }

        [Fact]
        public void OnDelete_removes_item_from_items_list()
        {
            var viewModel = new MainPageViewModel();
            MessagingCenter.Subscribe<MainPageViewModel, MsgArgs<TodoItem, ObservableCollection<TodoItem>>>(this, "OnDelete", (sender, message) =>
            {
                message.Object2.Remove(message.Object);
            });

            var item = new TodoItem();
            viewModel.Items.Add(item);
            viewModel.OnDeleteCommand.Execute(item);
            Assert.DoesNotContain(item, viewModel.Items);
        }
    }
}