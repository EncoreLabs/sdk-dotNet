using EncoreTickets.SDK.Inventory;
using System.Collections.Generic;
using System.Linq;
using System;
using EncoreTickets.SDK.Content;
using Product = EncoreTickets.SDK.Inventory.Models.Product;
using Product2 = EncoreTickets.SDK.Content.Models.Product;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Content.Models;
using EncoreTickets.SDK.Inventory.Models;

namespace SDKConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter username: ");
            var userName = Console.ReadLine();
            Console.Write("Enter Password: ");
            var password = Console.ReadLine();
            var context = new ApiContext(Environments.Sandbox, userName, password) {Affiliate = "encoretickets"};
            var productIds = new List<string>();

            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get JWT token for the venue service");
            Console.WriteLine(" ========================================================== ");
            var vsApi = new VenueServiceApi(context);
            var authContext = vsApi.AuthenticationService.Authenticate();

            Console.WriteLine($"username: {authContext.UserName}");
            Console.WriteLine($"Password: {authContext.Password}");
            Console.WriteLine($"token: {authContext.AccessToken}");
            Console.WriteLine($"authenticated: {vsApi.AuthenticationService.IsThereAuthentication()}");

            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get standard attributes ");
            Console.WriteLine(" ========================================================== ");
            var stas = vsApi.GetStandardAttributes();

            foreach (var a in stas)
            {
                Console.WriteLine($"{a.title} - {a.intention}");
            }

            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Update standard attribute by title ");
            Console.WriteLine(" ========================================================== ");
            var sourceAttribute = stas.First();
            var updatedAttribute = vsApi.UpsertStandardAttributeByTitle(sourceAttribute);
            Console.WriteLine($"{updatedAttribute?.title} - {updatedAttribute?.intention}");


            /* Get seat attributes */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get seat attributes for 163");
            Console.WriteLine(" ========================================================== ");            
            IList<EncoreTickets.SDK.Venue.Models.SeatAttribute> sas = vsApi.GetSeatAttributes("163");

            foreach (var a in sas)
            {
                Console.WriteLine(
                    string.Format("{0} - {1} [{2}-{3} : {4}]", 
                    a.seatIdentifier, 
                    a.attributes[0].title,
                    (!string.IsNullOrEmpty(a.startDate)) ? a.startDate : "-",
                    (!string.IsNullOrEmpty(a.endDate)) ? a.endDate : "-",
                    (a.performanceTimes != null) ? string.Join(",", a.performanceTimes) : "-"));
            }


            /* Upsert seat attributes */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Upsert seat attributes for 163");
            Console.WriteLine(" ========================================================== ");
            var result = vsApi.UpsertSeatAttributes("163", sas);
            Console.WriteLine(result);

            /* Get locations */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all venues ");
            Console.WriteLine(" ========================================================== ");
            IList<EncoreTickets.SDK.Venue.Models.Venue> venues = vsApi.GetVenues();

            foreach (var a in venues)
            {
                Console.WriteLine(string.Format("{0} ({1}): {2}", a.title, a.internalId, a.compositeId));
            }

            /* Get detailed venue by Id */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get detailed venue by ID = 55 ");
            Console.WriteLine(" ========================================================== ");
            const string venueId = "55";
            var venue = vsApi.GetVenueById(venueId);
            Console.WriteLine($"{venue.title} ({venue.internalId}): {venue.compositeId}");

            /* Update venue by id */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Update venue by ID = 55  ");
            Console.WriteLine(" ========================================================== ");
            var resultVenue = vsApi.UpdateVenueById(venue);
            Console.WriteLine($"{resultVenue.title} ({resultVenue.internalId}): {resultVenue.compositeId}");

            /* Get locations */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all locations ");
            Console.WriteLine(" ========================================================== ");
            ContentServiceApi cs = new ContentServiceApi(context);
            IList<Location> locations = cs.GetLocations();

            foreach (var a in locations)
            {
                Console.WriteLine(string.Format("{0} ({1}): {2}", a.name, a.isoCode, string.Join(",", a.subLocations.ConvertAll<string>(x => x.name))));
            }

            /* Get products */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all us products ");
            Console.WriteLine(" ========================================================== ");
            IList<Product2> products2 = cs.GetProducts();

            int count = 0;
            foreach (var p2 in products2)
            {
                Console.WriteLine(string.Format("{0} ({1}): {2}", p2.name, p2.id, p2.venue != null ? p2.venue.name : "- unknown-"));
                // get detailed information for every 5th product
                if (Math.DivRem(count, 5, out var temp) > 1)
                {
                    productIds.Add(p2.id);                    
                }
                count++;
                Console.WriteLine("-------");
            }
            
            /* Searching products test */
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Inventory service serch for products with 'w' ");
            Console.WriteLine(" ========================================================== ");
            InventoryServiceApi inventoryService = new InventoryServiceApi(context);
            IList<Product> products = inventoryService.Search("w");

            foreach (var p3 in products)
            {
                Console.WriteLine(p3.name);
            }

            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Inventory service serch for availability for products in array ");
            Console.WriteLine(" ========================================================== ");

            foreach (string pId in productIds)
            {
                Console.WriteLine(string.Format("--------* {0} *----------", pId));

                Product2 p = cs.GetProductById(pId);
                Console.WriteLine(string.Format("{0} ({1}): {2}", p.name, p.id, p.synopsis));

                IList<Performance> availability = inventoryService.GetPerformances(pId, 2, DateTime.Today, DateTime.Today.AddMonths(1));
                foreach (var a in availability)
                {
                    Console.WriteLine(string.Format("{0} - Tickets: {1}", a.datetime, a.largestLumpOfTickets));
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

                Console.WriteLine(string.Format("xxxxxxxxxxxxxx* {0} xxxxxxxxxxxxxx", pId));
            }            

            Console.WriteLine(" -- FINISHED --");
            Console.ReadLine();
        }
    }
}

