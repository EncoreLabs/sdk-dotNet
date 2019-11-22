using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using EncoreTickets.SDK.Api.Context;
using Microsoft.Extensions.Configuration;

namespace EncoreTickets.ConsoleTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = SetupConfiguration();
            await AwsTester.TestAws(configuration);

            var context = CreateApiContext(configuration["Venue:Username"], configuration["Venue:Password"]);
            PricingServiceTester.TestPricingService(configuration["Pricing:AccessToken"]);
            VenueServiceTester.TestVenueService(context);
            var productIds = ContentServiceTester.TestContentServiceAndGetProducts(context);
            InventoryServiceTester.TestInventoryService(context, productIds);
            BasketServiceTester.TestBasketService(context);

            Console.WriteLine();
            Console.WriteLine(" -- FINISHED --");
            Console.ReadLine();
        }

        private static IConfiguration SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.real.json", optional: true);
            return builder.Build();
        }

        private static ApiContext CreateApiContext(string userName, string password)
        {
            return new ApiContext(Environments.Sandbox, userName, password) { Affiliate = "encoretickets" };
        }
    }
}

