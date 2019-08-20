using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Content.Models;
using EncoreTickets.SDK.Interfaces;
using EncoreTickets.SDK.Venue.Models;
using NUnit.Framework;
using Product = EncoreTickets.SDK.Inventory.Models.Product;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiBaseEnumerableResponseTests
    {
        private static object[] sourceForDataTests =
        {
            new List<SeatAttribute> {new SeatAttribute(), new SeatAttribute()},
            new List<StandardAttribute> {new StandardAttribute()},
            new List<Product> {new Product(), new Product(), new Product()},
            new List<Location> {new Location(), new Location(), new Location()},
        };

        [TestCaseSource(nameof(sourceForDataTests))]
        public void Api_BaseEnumerableResponse_Data_IsCorrect<T>(List<T> data)
            where T : IObject
        {
            var response = new BaseEnumerableResponse<T> {response = data};
            var result = response.Data;
            Assert.AreEqual(data.Count, result.Count);
            foreach (var item in data)
            {
                Assert.IsTrue(result.Contains(item));
            }
        }

        [TestCaseSource(nameof(sourceForDataTests))]
        public void Api_BaseEnumerableResponse_GetEnumerator_ReturnsCorrectEnumerator<T>(List<T> data)
            where T : IObject
        {
            var response = new BaseEnumerableResponse<T> { response = data };
            foreach (var item in response)
            {
                Assert.IsTrue(item != null);
            }
        }
    }
}
