using EncoreTickets.SDK.Api.Results.Response;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api
{
    internal class ApiResponseTests
    {
        private static readonly object[] SourceForDataProperty =
        {
            new object[] {},
            new object(),
            "test"
        };

        [TestCaseSource(nameof(SourceForDataProperty))]
        public void Api_ApiResponse_DataProperty_ReturnsCorrectValue<T>(T instance)
            where T : class
        {
            var response = new ApiResponse<T> {response = instance};
            Assert.AreEqual(instance, response.Data);
        }
    }
}
