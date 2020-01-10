using System;
using EncoreTickets.SDK.Utilities.Exceptions;

namespace EncoreTickets.SDK.Utilities.CommonModels.Extensions
{
    public static class PriceWithCurrencyExtension
    {
        /// <summary>
        /// Returns the price in string format.
        /// </summary>
        /// <param name="price"></param>
        /// <returns>The user-friendly string with value and currency.</returns>
        public static string ToStringFormat<T>(this T price)
            where T : IPriceWithCurrency
        {
            var powerToConvertToRealValue = Math.Pow(10, price.DecimalPlaces ?? 2);
            var realValue = price.Value / powerToConvertToRealValue;
            var valueAsStr = realValue.HasValue && realValue - Math.Truncate(realValue.Value) > 0D
                ? $"{realValue:F20}".TrimEnd('0')
                : realValue.ToString();
            return $"{valueAsStr}{price.Currency}";
        }

        /// <summary>
        /// Adds two prices together.
        /// </summary>
        /// <param name="firstPrice"></param>
        /// <param name="secondPrice"></param>
        /// <returns>The Price where value is the sum of the two operands' values and the currency is the same as operands'.</returns>
        /// <exception cref="CurrenciesDontMatchException">When currencies or numbers of decimal places differ between operands.</exception>
        public static T Add<T>(this T firstPrice, T secondPrice) 
            where T : class, IPriceWithCurrency, new()
            => firstPrice.PerformArithmeticOperation(secondPrice, (x, y) => x + y);

        /// <summary>
        /// Subtracts one price from another.
        /// </summary>
        /// <param name="firstPrice"></param>
        /// <param name="secondPrice"></param>
        /// <returns>The Price where value is the difference of the first and second operand values and the currency is the same as operands'.</returns>
        /// <exception cref="CurrenciesDontMatchException">When currencies or numbers of decimal places differ between operands.</exception>
        public static T Subtract<T>(this T firstPrice, T secondPrice) 
            where T : class, IPriceWithCurrency, new() 
            => firstPrice.PerformArithmeticOperation(secondPrice, (x, y) => x - y);

        /// <summary>
        /// Multiplies a price by a number.
        /// </summary>
        /// <param name="price"></param>
        /// <param name="number"></param>
        /// <returns>The Price where value is the product of the original price and the number and the currency is the same as original price's.</returns>
        public static T MultiplyByNumber<T>(this T price, int number) 
            where T : class, IPriceWithCurrency, new() 
            => price != null 
                ? new T
                {
                    Currency = price.Currency,
                    DecimalPlaces = price.DecimalPlaces,
                    Value = (price.Value ?? 0) * number
                }
                : null;

        private static T PerformArithmeticOperation<T>(this T firstPrice, T secondPrice, Func<int, int, int> operation)
            where T : class, IPriceWithCurrency, new()
        {
            if (firstPrice == null || secondPrice == null)
            {
                return null;
            }
            if (firstPrice.Currency != secondPrice.Currency || firstPrice.DecimalPlaces != secondPrice.DecimalPlaces)
            {
                throw new CurrenciesDontMatchException();
            }
            return new T
            {
                Currency = firstPrice.Currency,
                DecimalPlaces = firstPrice.DecimalPlaces,
                Value = operation(firstPrice.Value ?? 0, secondPrice.Value ?? 0)
            };
        }
    }
}
