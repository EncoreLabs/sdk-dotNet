using System.Collections.Generic;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue.Models
{
    internal class VenuesResponseTests
    {
        [Test]
        public void Data_ReturnsSameVenuesAsInResponse()
        {
            var venue1 = new SDK.Venue.Models.Venue();
            var venue2 = new SDK.Venue.Models.Venue();
            var venues = new List<SDK.Venue.Models.Venue> {venue1, venue2};
            var response = new VenuesResponse {Response = new VenuesResponseContent {Results = venues}};

            var actual = response.Data;

            AssertExtension.AreObjectsValuesEqual(venues, actual);
        }
    }
}
