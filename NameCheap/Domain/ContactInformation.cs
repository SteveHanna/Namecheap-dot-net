using System;

namespace NameCheap
{
    public class ContactInformation : IEquatable<ContactInformation>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string StateProvince { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }

        public bool Equals(ContactInformation other)
            => string.Equals(this.FirstName, other?.FirstName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.LastName, other?.LastName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.Address1, other?.Address1, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.StateProvince, other?.StateProvince, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.City, other?.City, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.PostalCode, other?.PostalCode, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.Phone, other?.Phone, StringComparison.OrdinalIgnoreCase)
            && string.Equals(this.EmailAddress, other?.EmailAddress, StringComparison.OrdinalIgnoreCase);
    }
}
