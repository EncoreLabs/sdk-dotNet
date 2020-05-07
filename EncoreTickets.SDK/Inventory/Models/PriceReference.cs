using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class PriceReference
    {
        public Price FaceValue { get; set; }
        
        public Price CostPrice { get; set; }
        
        public Price SalePrice { get; set; }
        
        public Price OriginalSalePrice { get; set; }
        
        public FxRate FxRate { get; set; }
        
        public DateTime? Timestamp { get; set; }
    }
}