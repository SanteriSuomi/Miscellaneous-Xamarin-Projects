using SQLite;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Extensions;
using TodoList.Pages;
using Xamarin.Forms;

namespace TodoList.Data
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        private string image;
        public string Image
        {
            get => image;
            set
            {
                image = value;
                UpdateImageName(value);
            }
        }
        /// <summary>
        /// Return only the image name of Image.
        /// </summary>
        public string ImageName { get; private set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
        /// <summary>
        /// Return the date without the time part (only day/month/year).
        /// </summary>
        public string DateNoTime
        {
            get
            {
                if (string.IsNullOrEmpty(Date))
                {
                    return default;
                }

                return Date.Split(' ')[0];
            }
        }
        public string Time { get; set; }
        /// <summary>
        /// Date and time combined, separated by one whitespace character.
        /// </summary>
        public string DateAndTime
        {
            get
            {
                if (string.IsNullOrEmpty(Date)
                    || string.IsNullOrEmpty(Time))
                {
                    return string.Empty;
                }

                var dateWithoutTime = Date.Split(' ')[0];
                return $"{dateWithoutTime} {Time}";
            }
        }

        public ICommand LongPressCommand { get; }
        public ICommand ClickCommand { get; }

        private const double removeSelectionDelayTime = 0.4;

        public TodoItem()
        {
            LongPressCommand = new Command(() =>
            {
                MainPage.Instance.OnSelectionLongPressed(this).SafeFireAndForget();
            });

            ClickCommand = new Command<CollectionView>(HandleClickEvent);
        }

        private void HandleClickEvent(CollectionView cv)
        {
            if (cv.SelectionMode == SelectionMode.Multiple)
            {
                cv.SelectedItem = null;
                UpdateSelection(cv);
            }
            else
            {
                cv.SelectedItem = this;
                RemoveSelectionAfterDelay(cv, removeSelectionDelayTime).SafeFireAndForget(true);
            }
        }

        private void UpdateSelection(CollectionView cv)
        {
            try
            {
                var index = cv.SelectedItems.IndexOf(this);
                if (index >= 0)
                {
                    cv.SelectedItems.RemoveAt(index);
                }
                else
                {
                    cv.SelectedItems.Add(this);
                }
            }
            catch (Exception)
            {
                // To nothing on purpose.
            }
        }

        private async Task RemoveSelectionAfterDelay(CollectionView cv, double delay)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay)).ConfigureAwait(true);
            cv.SelectedItem = null;
        }

        private void UpdateImageName(string value)
        {
            try
            {
                var imageSplit = value.Split('/');
                ImageName = imageSplit[imageSplit.Length - 1];
            }
            catch (Exception)
            {
                ImageName = string.Empty;
            }
        }
    }
}