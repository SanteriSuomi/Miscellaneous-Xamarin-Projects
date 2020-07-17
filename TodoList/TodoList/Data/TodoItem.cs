using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TodoList.Extensions;
using TodoList.Pages;
using TodoList.Storage.ConfigSettings;
using Xamarin.Forms;

namespace TodoList.Data
{
    public class TodoItem : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ICommand LongPressCommand { get; }
        public ICommand ClickCommand { get; }

        public TodoItem()
        {
            LongPressCommand = new Command(() =>
            {
                MainPage.Instance.OnSelectionLongPressed(this).SafeFireAndForget();
            });

            ClickCommand = new Command<CollectionView>(HandleClickEvent);
        }

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
                NotifyPropertyChanged();
            }
        }

        public bool HasImage => !string.IsNullOrEmpty(image);

        private string imageName;
        /// <summary>
        /// Return only the image name of Image.
        /// </summary>
        public string ImageName
        {
            get => imageName;
            private set
            {
                imageName = value;
                NotifyPropertyChanged();
            }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        private string body;
        public string Body
        {
            get => body;
            set
            {
                body = value;
                NotifyPropertyChanged();
            }
        }

        private bool hasNotification;
        public bool HasNotification
        {
            get => hasNotification;
            set
            {
                hasNotification = value;
                NotifyPropertyChanged();
            }
        }
        public int NotificationId { get; set; }

        private string date;
        public string Date
        {
            get => date;
            set
            {
                date = value;
                NotifyPropertyChanged();
            }
        }

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

        private string time;
        public string Time
        {
            get => time;
            set
            {
                time = value;
                NotifyPropertyChanged();
            }
        }

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

        public (bool hasData, (DateTime date, TimeSpan time) dateAndTime) GetCombinedDateTime(bool useDateWithNoTime)
        {
            bool dateParsed;
            DateTime dateResult;
            if (useDateWithNoTime)
            {
                dateParsed = DateTime.TryParse(DateNoTime, out dateResult);
            }
            else
            {
                dateParsed = DateTime.TryParse(date, out dateResult);
            }

            if (dateParsed
                && TimeSpan.TryParse(time, out TimeSpan timeResult))
            {
                return (true, (dateResult, timeResult));
            }

            return (false, (default, default));
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
                RemoveSelectionAfterDelay(cv, Config.ST.SingleSelectRemoveDelay).SafeFireAndForget(true);
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