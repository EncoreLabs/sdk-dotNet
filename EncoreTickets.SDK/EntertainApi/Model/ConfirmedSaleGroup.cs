

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class ConfirmedSaleGroup
    {
        public string ShowName { get; set; }
        public string Referer { get; set; }
        //public List<ConfirmedSale> ConfirmedSales { get; set; }
        public decimal SaleAmountTotal { get; set; }
        public decimal SaleCommisionTotal { get; set; }
    }
}