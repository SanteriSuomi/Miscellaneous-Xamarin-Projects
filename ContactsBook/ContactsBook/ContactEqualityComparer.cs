using ContactsBook.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ContactsBook
{
    public class ContactEqualityComparer : IEqualityComparer<Contact>
    {
        public bool Equals(Contact x, Contact y)
        {
            return (NormalizeString(x.FullName) == NormalizeString(y.FullName))
                || (NormalizeString(x.PhoneNumber) == NormalizeString(y.PhoneNumber));
        }

        // Remove white-spaces and make lower case.
        private string NormalizeString(string name)
            => Regex.Replace(name, @"\s+", "").ToLowerInvariant();

        public int GetHashCode(Contact obj) => obj.FullName.GetHashCode();
    }
}