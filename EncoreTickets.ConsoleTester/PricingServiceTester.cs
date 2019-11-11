using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Pricing;

namespace EncoreTickets.ConsoleTester
{
    static class PricingServiceTester
    {
        public static void TestPricingService()
        {
            var apiKey = "access_token"; // it's too big for Console.ReadLine()
            var contextPricingService = new ApiContext(Environments.QA, apiKey);

            var pricingApi = new PricingServiceApi(contextPricingService);

            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get exchange rates");
            Console.WriteLine(" ========================================================== ");
            var rates = pricingApi.GetExchangeRates(null);
            foreach (var a in rates)
            {
                Console.WriteLine($"{a.baseCurrency} -> {a.targetCurrency}: {a.rate}, {a.encoreRate}, {a.protectionMargin}");
            }
        }
    }
}
