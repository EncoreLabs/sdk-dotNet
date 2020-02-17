using System;
using System.Globalization;

namespace EncoreTickets.SDK.Utilities.BusinessHelpers
{
    public static class MoneyHelper
    {
        public const int DefaultDecimalPlaces = 2;

        public static decimal ConvertFromIntRepresentationToDecimal(int sourceAmount, int? decimalPlaces)
        {
            var power = GetDecimalPowerForConversion(decimalPlaces);
            return sourceAmount / power;
        }

        public static int ConvertFromDecimalRepresentationToInt(decimal sourceAmount, int? decimalPlaces)
        {
            var power = GetDecimalPowerForConversion(decimalPlaces);
            return Convert.ToInt32(sourceAmount * power);
        }

        public static string ConvertFromDecimalRepresentationToString(decimal sourceAmount, int? decimalPlaces)
        {
            var places = GetDecimalPlaces(decimalPlaces);
            return sourceAmount.ToString($"F{places}", CultureInfo.InvariantCulture);
        }

        private static decimal GetDecimalPowerForConversion(int? decimalPlaces)
        {
            var places = GetDecimalPlaces(decimalPlaces);
            return GetDecimalPowerForConversion(places);
        }
        
        private static int GetDecimalPlaces(int? decimalPlaces)
        {
            return decimalPlaces ?? DefaultDecimalPlaces;
        }

        private static decimal GetDecimalPowerForConversion(int decimalPlaces)
        {
            return (decimal) Math.Pow(10, decimalPlaces);
        }
    }
}
