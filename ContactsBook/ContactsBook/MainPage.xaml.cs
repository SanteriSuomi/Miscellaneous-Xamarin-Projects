using ContactsBook.Data;
using ContactsBook.Data.Lists;
using ContactsBook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ContactsBook
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<ContactList> ListOfContacts { get; set; }

        private const float DELAY_UNTIL_SELECTEDITEM_NULL = 0.3f;
        private bool contactActionsDisplayed;

        #region Refreshing
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand => new Command(async () =>
                                                {
                                                    IsRefreshing = true;
                                                    await InitializeContacts(() => BindingContext = this).ConfigureAwait(true);
                                                    IsRefreshing = false;
                                                });
        #endregion

        public MainPage()
        {
            InitializeComponent();
            InitializeContacts(() => BindingContext = this).ConfigureAwait(true);
        }

        #region Contact Initialization
        private async Task InitializeContacts(Action finishAction)
        {
            await CheckAndRequestPermission(new Permissions.ContactsRead()).ConfigureAwait(true);
            await CheckAndRequestPermission(new Permissions.ContactsWrite()).ConfigureAwait(true);

            List<Contact> contacts = DependencyService.Get<IGetContactsService>().GetContacts();
            ListOfContacts = PopulateListOfContactsWithContacts(contacts);
            finishAction();
        }

        private static async Task<PermissionStatus> CheckAndRequestPermission<T>(T permission) where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync().ConfigureAwait(false);
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync().ConfigureAwait(false);
            }

            return status;
        }

        private ObservableCollection<ContactList> PopulateListOfContactsWithContacts(List<Contact> contacts)
        {
            List<ContactList> contactLists = new List<ContactList>();
            for (int i = 0; i < contacts.Count; i++)
            {
                Contact currentContact = contacts[i];
                int contactListIndex = contactLists.FindIndex(c => c.FirstLetter == currentContact.FirstLetter);
                bool listHasGroupKey = contactListIndex >= 0;

                if (listHasGroupKey)
                {
                    contactLists[contactListIndex].Add(currentContact);
                }
                else
                {
                    InitializeNewContactList(contactLists, currentContact);
                }
            }

            return new ObservableCollection<ContactList>(contactLists);
        }

        private static void InitializeNewContactList(List<ContactList> contactLists, Contact currentContact)
        {
            ContactList contactList = new ContactList
            {
                FirstLetter = currentContact.FirstLetter.ToUpperInvariant()
            };

            contactList.Add(currentContact);
            contactLists.Add(contactList);
        }
        #endregion

        #region Contact Actions
        private async void OnContactClicked(object sender, SelectedItemChangedEventArgs args)
        {
            await Task.Delay(TimeSpan.FromSeconds(DELAY_UNTIL_SELECTEDITEM_NULL)).ConfigureAwait(true);
            Device.BeginInvokeOnMainThread(() => mainListView.SelectedItem = null);

            if (!contactActionsDisplayed)
            {
                contactActionsDisplayed = true;
                await DisplayContactActions(args).ConfigureAwait(true);
                contactActionsDisplayed = false;
            }
        }

        private async Task DisplayContactActions(SelectedItemChangedEventArgs args)
        {
            var contact = args.SelectedItem as Contact;
            var contactNameTrimmed = contact.FullName.Trim();

            string action = await DisplayActionSheet($"Contact {contactNameTrimmed}", "Cancel", null, "Call", "Message").ConfigureAwait(true);
            switch (action)
            {
                case "Call":
                    await AttemptCommunication(() =>
                    {
                        PhoneDialer.Open(contact.PhoneNumber);
                        return null;
                    },
                    contactNameTrimmed,
                    "Call").ConfigureAwait(true);
                    break;

                case "Message":
                    await AttemptCommunication(async () =>
                    {
                        await Sms.ComposeAsync(new SmsMessage()
                        {
                            Body = string.Empty,
                            Recipients = new List<string>()
                            {
                                contact.PhoneNumber
                            }
                        }).ConfigureAwait(true);
                    },
                    contactNameTrimmed,
                    "Message").ConfigureAwait(true);
                    break;
            }
        }

        private async Task AttemptCommunication(Func<Task> communicationMethod, string contactNameTrimmed, string communicationName)
        {
            bool confirmation = await DisplayAlert($"{communicationName} Confirmation", $"Are you sure you want to {communicationName} {contactNameTrimmed}?", "Accept", "Cancel").ConfigureAwait(true);
            if (confirmation)
            {
                try
                {
                    await communicationMethod().ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //await DisplayAlert($"{communicationName} Error", $"{communicationName} unsuccessful. Please check the number provided.", "Cancel").ConfigureAwait(true);
                }
            }
        }
        #endregion
    }
}