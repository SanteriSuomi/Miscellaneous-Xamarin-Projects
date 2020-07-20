using SimpleTodoListMVVM.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleTodoListMVVM
{
    public class MainPageViewModel : BindableObject
    {
        public MainPageViewModel()
        {
            Items = new ObservableCollection<TodoItem>(TodoItem.GetSavedItems());
            OnNewItemCommand = new Command(OnNewItem);
            OnCompleteCommand = new Command<TodoItem>(OnComplete);
            OnDeleteCommand = new Command<TodoItem>(OnDelete);
        }

        public ObservableCollection<TodoItem> Items { get; set; }
        public string PageTitle { get; set; }

        private TodoItem _selectedItem;
        public TodoItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                PageTitle = value?.Title;
                OnPropertyChanged();
            }
        }

        public ICommand OnNewItemCommand { get; set; }
        public ICommand OnCompleteCommand { get; set; }
        public ICommand OnDeleteCommand { get; set; }

        private void OnNewItem()
        {
            MessagingCenter.Send(this, "OnNewItem", new MsgArgs<ObservableCollection<TodoItem>>()
            {
                Object = Items
            });
        }

        private void OnComplete(TodoItem newItem)
        {
            newItem.IsComplete = true;
            MoveToEndOfList(newItem);
        }

        private void MoveToEndOfList(TodoItem newItem)
        {
            var lastIndex = Items.Count - 1;
            var lastItem = Items[lastIndex];
            var newIndex = Items.IndexOf(newItem);
            if (newIndex >= 0)
            {
                Items[lastIndex] = newItem;
                Items[newIndex] = lastItem;
            }
        }

        private void OnDelete(TodoItem item)
        {
            MessagingCenter.Send(this, "OnDelete", new MsgArgs<TodoItem, ObservableCollection<TodoItem>>()
            {
                Object = item,
                Object2 = Items
            });
        }
    }
}