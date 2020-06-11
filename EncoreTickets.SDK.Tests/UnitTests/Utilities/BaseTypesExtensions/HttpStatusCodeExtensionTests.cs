using System.Net;
using EncoreTickets.SDK.Utilities.BaseTypesExtensions;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.BaseTypesExtensions
{
    [TestFixture]
    internal class HttpStatusCodeExtensionTests
    {
        [TestCase(HttpStatusCode.NotFound, false)]
        [TestCase(405, false)]
        [TestCase(HttpStatusCode.OK, false)]
        [TestCase(HttpStatusCode.InternalServerError, true)]
        [TestCase(HttpStatusCode.ServiceUnavailable, true)]
        public void IsServerError(HttpStatusCode code, bool expected)
        {
            var actual = code.IsServerError();

            Assert.AreEqual(expected, actual);
        }
    }
}
