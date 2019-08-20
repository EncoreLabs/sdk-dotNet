using System.Collections.Generic;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Venue
{
    internal class VenueSeatAttributeResponseTests
    {
        [Test]
        public void Venue_SeatAttributeResponse_Data_IsCorrect()
        {
            var firstSeatAttribute = new SeatAttribute();
            var secondSeatAttribute = new SeatAttribute();
            var response = new SeatAttributeResponse
            {
                response = new List<SeatAttribute> {firstSeatAttribute , secondSeatAttribute }
            };
            var result = response.Data;
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(firstSeatAttribute ));
            Assert.IsTrue(result.Contains(secondSeatAttribute ));
        }

        [Test]
        public void Venue_SeatAttributeResponse_GetEnumerator_ReturnsCorrectEnumerator()
        {
            var firstSeatAttribute = new SeatAttribute();
            var secondSeatAttribute = new SeatAttribute();
            var response = new SeatAttributeResponse
            {
                response = new List<SeatAttribute> { firstSeatAttribute, secondSeatAttribute }
            };
            foreach (var item in response)
            {
                Assert.IsTrue(item != null);
            }
        }
    }
}
