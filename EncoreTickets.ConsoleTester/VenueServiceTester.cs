using System;
using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;

namespace EncoreTickets.ConsoleTester
{
    class VenueServiceTester
    {
        public static void TestVenueService(ApiContext context)
        {
            var venueServiceApi = TestVenueServiceAuthentication(context);
            var standardAttributes = TestGetStandardAttributes(venueServiceApi);
            TestUpdateStandardAttributeByTitle(venueServiceApi, standardAttributes);
            var seatAttributes = TestGetSeatAttributes(venueServiceApi, "163");
            TestUpsertSeatAttributes(venueServiceApi, "163", seatAttributes);
            TestGetVenues(venueServiceApi);
            var venue = TestGetVenueById(venueServiceApi, "55");
            TestUpdateVenue(venueServiceApi, venue);
        }

        private static VenueServiceApi TestVenueServiceAuthentication(ApiContext context)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get JWT token for the venue service");
            Console.WriteLine(" ========================================================== ");
            var venueServiceApi = new VenueServiceApi(context);
            var authContext = venueServiceApi.AuthenticationService.Authenticate();

            Console.WriteLine($"username: {authContext.UserName}");
            Console.WriteLine($"Password: {authContext.Password}");
            Console.WriteLine($"token: {authContext.AccessToken}");
            Console.WriteLine($"authenticated: {venueServiceApi.AuthenticationService.IsThereAuthentication()}");

            return venueServiceApi;
        }

        private static IList<StandardAttribute> TestGetStandardAttributes(VenueServiceApi venueServiceApi)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get standard attributes ");
            Console.WriteLine(" ========================================================== ");
            var standardAttributes = venueServiceApi.GetStandardAttributes();

            foreach (var a in standardAttributes)
            {
                Console.WriteLine($"{a.title} - {a.intention}");
            }

            return standardAttributes;
        }

        private static void TestUpdateStandardAttributeByTitle(VenueServiceApi venueServiceApi, IList<StandardAttribute> standardAttributes)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Update standard attribute by title ");
            Console.WriteLine(" ========================================================== ");
            var sourceAttribute = standardAttributes.First();
            var updatedAttribute = venueServiceApi.UpsertStandardAttributeByTitle(sourceAttribute);
            Console.WriteLine($"{updatedAttribute?.title} - {updatedAttribute?.intention}");
        }

        private static IList<SeatAttribute> TestGetSeatAttributes(VenueServiceApi venueServiceApi, string venueId)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine($" Test: Get seat attributes for {venueId}");
            Console.WriteLine(" ========================================================== ");
            IList<SeatAttribute> seatAttributes = venueServiceApi.GetSeatAttributes(venueId);

            foreach (var a in seatAttributes)
            {
                Console.WriteLine(
                    $"{a.seatIdentifier} - {a.attributes[0].title} [{(!string.IsNullOrEmpty(a.startDate) ? a.startDate : "-")}-{((!string.IsNullOrEmpty(a.endDate)) ? a.endDate : "-")} : {(a.performanceTimes != null ? string.Join(",", a.performanceTimes) : "-")}]");
            }

            return seatAttributes;
        }

        private static void TestUpsertSeatAttributes(VenueServiceApi venueServiceApi, string venueId, IList<SeatAttribute> seatAttributes)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine($" Test: Upsert seat attributes for {venueId}");
            Console.WriteLine(" ========================================================== ");
            var result = venueServiceApi.UpsertSeatAttributes(venueId, seatAttributes);
            Console.WriteLine(result);
        }

        private static void TestGetVenues(VenueServiceApi venueServiceApi)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine(" Test: Get all venues ");
            Console.WriteLine(" ========================================================== ");
            IList<Venue> venues = venueServiceApi.GetVenues();

            foreach (var a in venues)
            {
                Console.WriteLine($"{a.title} ({a.internalId}): {a.compositeId}");
            }
        }

        private static Venue TestGetVenueById(VenueServiceApi venueServiceApi, string venueId)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine($" Test: Get detailed venue by ID = {venueId} ");
            Console.WriteLine(" ========================================================== ");
            var venue = venueServiceApi.GetVenueById(venueId);
            Console.WriteLine($"{venue.title} ({venue.internalId}): {venue.compositeId}");
            return venue;
        }

        private static void TestUpdateVenue(VenueServiceApi venueServiceApi, Venue venue)
        {
            Console.WriteLine();
            Console.WriteLine(" ========================================================== ");
            Console.WriteLine($" Test: Update venue by ID = {venue.internalId}  ");
            Console.WriteLine(" ========================================================== ");
            var resultVenue = venueServiceApi.UpdateVenueById(venue);
            Console.WriteLine($"{resultVenue.title} ({resultVenue.internalId}): {resultVenue.compositeId}");
        }
    }
}
