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
            var seatAttribute1 = new SeatAttribute();
            var seatAttribute2 = new SeatAttribute();
            var response = new SeatAttributeResponse
            {
                response = new List<SeatAttribute> {seatAttribute1, seatAttribute2}
            };
            var result = response.Data;
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(seatAttribute1));
            Assert.IsTrue(result.Contains(seatAttribute2));
        }

        [Test]
        public void Venue_SeatAttributeResponse_GetEnumerator_ReturnsCorrectEnumerator()
        {
            var seatAttribute1 = new SeatAttribute();
            var seatAttribute2 = new SeatAttribute();
            var response = new SeatAttributeResponse
            {
                response = new List<SeatAttribute> { seatAttribute1, seatAttribute2 }
            };
            foreach (var item in response)
            {
                Assert.IsTrue(item != null);
            }
        }
    }
}
