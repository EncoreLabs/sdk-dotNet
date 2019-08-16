using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.v1
{
    internal class ApiResponseTests
    {
        [TestCase(42)]
        [TestCase("string")]
        [TestCase(double.Epsilon)]
        public void ApiResponse_Constructor_InitializesDataProperty<T>(T instance)
        {
            var response = new ApiResponse<T>(instance);
            Assert.AreEqual(instance, response.Data);
        }
    }
}
