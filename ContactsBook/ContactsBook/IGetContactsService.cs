using ContactsBook.Data;
using System.Collections.Generic;

namespace ContactsBook.Services
{
    public interface IGetContactsService
    {
        List<Contact> GetContacts();
    }
}