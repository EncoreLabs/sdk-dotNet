using System;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api
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
            new object[]
            {
                new ApiContext(Environments.Staging),
                "https://venue-service.stagingtixuk.io/api/"
            },
            new object[]
            {
                new ApiContext(Environments.QA),
                "https://venue-service.qatixuk.io/api/"
            },
        };

        public ApiBaseApiTests() : base(new ApiContext(), testHost)
        {
        }

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
    }
}
