using System.Collections.Generic;
using EncoreTickets.SDK.Api.Results.Response;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Results.Response
{
    internal class ApiResponseTests
    {
        [TestCaseSource(typeof(ApiResponseTestsSource), nameof(ApiResponseTestsSource.Data_ReturnsSameObjectAsInResponse))]
        public void Data_ReturnsSameObjectAsInResponse<T>(T instance)
        {
            var response = new ApiResponse<T> { Response = instance };

            Assert.AreEqual(instance, response.Data);
        }
    }

    internal static class ApiResponseTestsSource
    {
        public static IEnumerable<TestCaseData> Data_ReturnsSameObjectAsInResponse { get; } = new[]
        {
            new TestCaseData(new int[] { }),
            new TestCaseData(new object()),
            new TestCaseData("test"),
            new TestCaseData(45.89),
        };
    }
}
