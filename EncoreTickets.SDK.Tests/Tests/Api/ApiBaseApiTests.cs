using System;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiBaseApiTests : BaseApi
    {
        private static readonly string testHost = "venue-service.{0}tixuk.io/api/";

        private static readonly object[] SourceForBaseUrlTest =
        {
            new object[]
            {
                new ApiContext(Environments.Production),
                "https://venue-service.tixuk.io/api/"
            },
            new object[]
            {
                new ApiContext(Environments.Sandbox),
                "https://venue-service.devtixuk.io/api/"
            },
        };

        [TestCaseSource(nameof(SourceForBaseUrlTest))]
        public void Api_BaseApi_BaseUrl_ReturnsCorrectly(ApiContext context, string expected)
        {
            Context = context;
            Assert.DoesNotThrow(() => new Uri(BaseUrl, UriKind.Absolute));
            Assert.AreEqual(expected, BaseUrl);
        }

        [Test]
        public void Api_BaseApi_Executor_ReturnsNew()
        {
            var firstExecutor = Executor;
            var secondExecutor = Executor;
            Assert.AreNotEqual(firstExecutor, secondExecutor);
        }

        public ApiBaseApiTests() : base(new ApiContext(), testHost)
        {
        }
    }
}
