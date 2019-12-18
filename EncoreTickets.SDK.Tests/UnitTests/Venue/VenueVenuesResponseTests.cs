using System.Collections.Generic;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue
{
    internal class VenueVenuesResponseTests
    {
        [Test]
        public void Venue_VenuesResponse_Data_IsCorrect()
        {
            var venue1 = new SDK.Venue.Models.Venue();
            var venue2 = new SDK.Venue.Models.Venue();
            var response = new VenuesResponse
            {
                Response = new VenuesResponseContent
                {
                    Results = new List<SDK.Venue.Models.Venue> { venue1, venue2 }
                }
            };

            var result = response.Data;

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(venue1));
            Assert.IsTrue(result.Contains(venue2));
        }
    }
}
