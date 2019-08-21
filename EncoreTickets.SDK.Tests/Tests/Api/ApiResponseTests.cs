using EncoreTickets.SDK.Api.Results;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiResponseTests
    {
        [TestCase(42)]
        [TestCase("string")]
        [TestCase(double.Epsilon)]
        public void Api_ApiResponse_Constructor_InitializesDataProperty<T>(T instance)
        {
            var response = new ApiResponse<T>(instance);
            Assert.AreEqual(instance, response.Data);
        }
    }
}
