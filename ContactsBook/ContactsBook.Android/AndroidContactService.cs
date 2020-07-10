using Android.Provider;
using ContactsBook.Data;
using ContactsBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

[assembly: Dependency(typeof(ContactsBook.Droid.Services.AndroidContactService))]
namespace ContactsBook.Droid.Services
{
    public class AndroidContactService : IGetContactsService
    {
        public List<Contact> GetContacts()
        {
            HashSet<Contact> contacts = new HashSet<Contact>(new ContactEqualityComparer());
            using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null))
            {
                if (phones == null) return new List<Contact>();

                while (phones.MoveToNext())
                {
                    try
                    {
                        string fullName = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                        string[] names = fullName.Split(' ');

                        string firstName = names[0];

                        string lastName;
                        if (names.Length > 1)
                        {
                            lastName = names[1];
                        }
                        else
                        {
                            lastName = string.Empty;
                        }

                        string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));

                        contacts.Add(new Contact
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            PhoneNumber = phoneNumber,
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return contacts.ToList();
        }
    }
}