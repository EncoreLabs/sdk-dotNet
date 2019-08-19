using System.Collections.Generic;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Venue
{
    internal class VenueStandardAttributeResponseTests
    {
        [Test]
        public void Venue_StandardAttributeResponse_Data_IsCorrect()
        {
            var attribute1 = new StandardAttribute();
            var attribute2 = new StandardAttribute();
            var response = new StandardAttributeResponse
            {
                response = new List<StandardAttribute> {attribute1, attribute2}
            };
            var result = response.Data;
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(attribute1));
            Assert.IsTrue(result.Contains(attribute2));
        }

        [Test]
        public void Venue_StandardAttributeResponse_GetEnumerator_ReturnsCorrectEnumerator()
        {
            var attribute1 = new StandardAttribute();
            var attribute2 = new StandardAttribute();
            var response = new StandardAttributeResponse
            {
                response = new List<StandardAttribute> { attribute1, attribute2 }
            };
            foreach (var item in response)
            {
                Assert.IsTrue(item != null);
            }
        }
    }
}
