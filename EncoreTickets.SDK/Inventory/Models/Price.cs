using System;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price
    {
        public int? value { get; set; }

        public string currency { get; set; }

        public int? decimalPlaces { get; set; }

        public override string ToString()
        {
            return $"{currency}{value / 100}";
        }

        public static Price operator +(Price firstPrice, Price secondPrice)
        {
            if (firstPrice.currency != secondPrice.currency || firstPrice.decimalPlaces != secondPrice.decimalPlaces)
            {
                throw new Exception("Currencies do not match.");
            }
            return new Price
            {
                currency = firstPrice.currency,
                decimalPlaces = firstPrice.decimalPlaces,
                value = (firstPrice.value ?? 0) + (secondPrice.value ?? 0)
            };
        }

        public static Price operator -(Price firstPrice, Price secondPrice)
        {
            if (firstPrice.currency != secondPrice.currency || firstPrice.decimalPlaces != secondPrice.decimalPlaces)
            {
                throw new Exception("Currencies do not match.");
            }
            return new Price
            {
                currency = firstPrice.currency,
                decimalPlaces = firstPrice.decimalPlaces,
                value = (firstPrice.value ?? 0) - (secondPrice.value ?? 0)
            };
        }

        public static Price operator *(Price price, int quantity)
        {
            return new Price
            {
                currency = price.currency,
                decimalPlaces = price.decimalPlaces,
                value = (price.value ?? 0) * quantity
            };
        }
    }
}