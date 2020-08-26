using System.Collections.Generic;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class PartnerGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Partner> Partners { get; set; }
    }
}
