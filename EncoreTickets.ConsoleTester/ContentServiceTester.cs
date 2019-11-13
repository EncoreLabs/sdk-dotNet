using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Content;
using EncoreTickets.SDK.Content.Models;
using Product = EncoreTickets.SDK.Content.Models.Product;

namespace EncoreTickets.ConsoleTester
{
    static class ContentServiceTester
    {
        public static List<string> TestContentServiceAndGetProducts(ApiContext context)
        {
            var contentServiceApi = new ContentServiceApi(context);
            TestGetLocations(contentServiceApi);
            return TestGetProducts(contentServiceApi);
        }

        private static void TestGetLocations(ContentServiceApi contentServiceApi)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all locations ");
            Console.WriteLine(" ========================================================== ");
            IList<Location> locations = contentServiceApi.GetLocations();

            foreach (var a in locations)
            {
                Console.WriteLine($"{a.name} ({a.isoCode}): {string.Join(",", a.subLocations.ConvertAll<string>(x => x.name))}");
            }
        }

        private static List<string> TestGetProducts(ContentServiceApi contentServiceApi)
        {
            var productIds = new List<string>();
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all us products ");
            Console.WriteLine(" ========================================================== ");
            IList<Product> products2 = contentServiceApi.GetProducts();

            int count = 0;
            foreach (var p2 in products2)
            {
                Console.WriteLine($"{p2.name} ({p2.id}): {(p2.venue != null ? p2.venue.name : "- unknown-")}");
                // get detailed information for every 5th product
                if (Math.DivRem(count, 5, out var temp) > 1)
                {
                    productIds.Add(p2.id);
                }
                count++;
                Console.WriteLine("-------");
            }

            return productIds;
        }
    }
}
