using System.Collections.Generic;

namespace EncoreTickets.SDK.EntertainApi.Model
{
    public class ShoppingBasket
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public int ItemCount { get; set; }
        public List<BasketItemHistory> BasketItemHistories { get; set; }

        public ShoppingBasket()
        {
            BasketItemHistories = new List<BasketItemHistory>();
        }

        #region Helper Properties

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Password);
        }
        
        #endregion
    }
}