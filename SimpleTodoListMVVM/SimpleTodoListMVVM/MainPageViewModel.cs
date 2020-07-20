using SimpleTodoListMVVM.Extensions;
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
            UpdateCompletedHeaderText(0);
        }

        public ObservableCollection<TodoItem> Items { get; set; }

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

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

        private int _completedAmount;
        private void SetCompletedAmount(int value)
        {
            UpdateCompletedHeaderText(value);
            _completedAmount = value;
            CalculateAndSetProgress();
        }

        private float completedProgress;
        public float CompletedProgress
        {
            get => completedProgress;
            set
            {
                completedProgress = value;
                OnPropertyChanged();
            }
        }

        private string _completedHeaderText;
        public string CompletedHeaderText
        {
            get => _completedHeaderText;
            set
            {
                _completedHeaderText = value;
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
                Object = Items,
                CompleteHandler = () =>
                {
                    CalculateAndSetProgress();
                    UpdateCompletedHeaderText(_completedAmount);
                }
            });
        }

        private void OnComplete(TodoItem newItem)
        {
            newItem.IsComplete = true;
            SetCompletedAmount(_completedAmount + 1);
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
            if (item.IsComplete)
            {
                SetCompletedAmount(_completedAmount - 1);
            }

            MessagingCenter.Send(this, "OnDelete", new MsgArgs<TodoItem, ObservableCollection<TodoItem>>()
            {
                Object = item,
                Object2 = Items
            });
        }

        private void UpdateCompletedHeaderText(int value)
        {
            CompletedHeaderText = $"{value} out of {Items.Count} completed.";
        }

        private void CalculateAndSetProgress()
        {
            CompletedProgress = ((float)_completedAmount / Items.Count).Clamp(0, 1);
        }
    }
}