using System;
using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = CreateApiContext();
            PricingServiceTester.TestPricingService();
            VenueServiceTester.TestVenueService(context);
            var productIds = ContentServiceTester.TestContentServiceAndGetProducts(context);
            InventoryServiceTester.TestInventoryService(context, productIds);
            BasketServiceTester.TestBasketService(context);

            Console.WriteLine();
            Console.WriteLine(" -- FINISHED --");
            Console.ReadLine();
        }

        private static ApiContext CreateApiContext()
        {
            Console.WriteLine();
            Console.Write("Enter venue username: ");
            var userName = Console.ReadLine();
            Console.Write("Enter venue Password: ");
            var password = Console.ReadLine();
            return new ApiContext(Environments.Sandbox, userName, password) { Affiliate = "encoretickets" };
        }
    }
}

