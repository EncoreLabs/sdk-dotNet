using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Content;
using EncoreTickets.SDK.Inventory;
using EncoreTickets.SDK.Inventory.Models;
using Product = EncoreTickets.SDK.Content.Models.Product;

namespace EncoreTickets.ConsoleTester
{
    static class InventoryServiceTester
    {
        public static void TestInventoryService(ApiContext context, List<string> productIds)
        {
            var inventoryService = new InventoryServiceApi(context);
            TestSearchProducts(inventoryService, "w");
            TestAvailabilitySearch(inventoryService, context, productIds);
        }

        private static void TestSearchProducts(InventoryServiceApi inventoryService, string text)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine($" Test: Inventory service search for products with '{text}' ");
            Console.WriteLine(" ========================================================== ");
            
            IList<SDK.Inventory.Models.Product> products = inventoryService.Search(text);

            foreach (var p3 in products)
            {
                Console.WriteLine(p3.name);
            }
        }

        private static void TestAvailabilitySearch(InventoryServiceApi inventoryService, ApiContext context, List<string> productIds)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Inventory service search for availability for products in array ");
            Console.WriteLine(" ========================================================== ");

            foreach (string pId in productIds)
            {
                Console.WriteLine($"--------* {pId} *----------");

                var contentServiceApi = new ContentServiceApi(context);
                Product p = contentServiceApi.GetProductById(pId);
                Console.WriteLine($"{p.name} ({p.id}): {p.synopsis}");

                IList<Performance> availability = inventoryService.GetPerformances(pId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
                foreach (var a in availability)
                {
                    Console.WriteLine($"{a.datetime} - Tickets: {a.largestLumpOfTickets}");
                }

                if (availability.Count > 0)
                {
                    Console.WriteLine("--------* Availability *--------");
                    Availability seats = inventoryService.GetAvailability(pId, 2, availability.FirstOrDefault().datetime);
                    if (seats != null)
                    {
                        foreach (var a in seats.areas)
                        {
                            Console.WriteLine(a.name + " " + a.itemReference + " " + a.availableCount.ToString());
                        }
                    }
                }

                Console.WriteLine($"xxxxxxxxxxxxxx* {pId} xxxxxxxxxxxxxx");
            }
        }
    }
}
