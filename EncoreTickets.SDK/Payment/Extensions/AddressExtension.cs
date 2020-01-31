using EncoreTickets.SDK.Payment.Models;

namespace EncoreTickets.SDK.Payment.Extensions
{
    public static class AddressExtension
    {
        public static Address ToNullIfEmpty(this Address address)
        {
            if (address != null &&
                address.CountryCode == null &&
                address.PostalCode == null &&
                address.City == null &&
                address.Line1 == null &&
                address.Line2 == null &&
                address.StateOrProvince == null)
            {
                return null;
            }

            return address;
        }
    }
}
