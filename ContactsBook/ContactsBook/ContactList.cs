using System.Collections.ObjectModel;

namespace ContactsBook.Data.Lists
{
    public class ContactList : ObservableCollection<Contact>
    {
        /// <summary>
        /// ListView grouping key.
        /// </summary>
        public string FirstLetter { get; set; }
    }
}