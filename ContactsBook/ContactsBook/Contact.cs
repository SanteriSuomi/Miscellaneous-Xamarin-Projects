namespace ContactsBook.Data
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        /// <summary>
        /// Return the full name (FirstName + LastName) of this contact.
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Attempt to return the first letter of the name of this contact.
        /// </summary>
        public string FirstLetter
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                {
                    return string.Empty;
                }

                return FirstName[0].ToString();
            }
        }
    }
}