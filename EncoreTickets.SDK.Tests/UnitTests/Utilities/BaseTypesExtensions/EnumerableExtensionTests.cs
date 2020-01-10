using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    public class EnumerableExtensionTests
    {
        [Test]
        public void DistinctBy_Null()
        {
            var source = (IEnumerable<dynamic>) null;

            var result = source.DistinctBy(o => o.Id);

            Assert.Null(result);
        }

        [Test]
        public void DistinctBy_Empty()
        {
            var source = Enumerable.Empty<dynamic>();

            var result = source.DistinctBy(o => o.Id);

            Assert.IsEmpty(result);
        }

        [Test]
        public void DistinctBy_Successful()
        {
            var source = new List<dynamic> { new { Id = 1, Value = 5 }, new { Id = 1, Value = 7 }, new { Id = 2, Value = 5 } };

            var result = source.DistinctBy(o => o.Id).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(source[0], result[0]);
            Assert.AreEqual(source[2], result[1]);
        }
    }
}
