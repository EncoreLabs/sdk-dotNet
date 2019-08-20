namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price
    {
        public int? value { get; set; }

        public string currency { get; set; }

        public override string ToString()
        {
            return $"{currency}{value / 100}";
        }
    }
}