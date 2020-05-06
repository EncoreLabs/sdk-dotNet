using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Tests.Helpers.ApiWrappers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api
{
    internal class BaseApiTests : BaseApi
    {
        public override int? ApiVersion { get; }

        public BaseApiTests() : base(new ApiContext(), BaseApiTestsSource.TestHost)
        {
        }

        [Test]
        public void Constructor_InitializesBaseParameters()
        {
            var context = new ApiContext();
            var host = "some_host";

            var api = new BaseApiWrapper(context, host);

            Assert.AreEqual(context, api.SourceContext);
            Assert.AreEqual(host, api.SourceHost);
        }

        [TestCaseSource(typeof(BaseApiTestsSource), nameof(BaseApiTestsSource.BaseUrl_ReturnsCorrectly))]
        public void BaseUrl_ReturnsCorrectly(ApiContext context, string expected)
        {
            Context = context;

            Assert.DoesNotThrow(() =>
            {
                var url = new Uri(BaseUrl, UriKind.Absolute);
            });
            Assert.AreEqual(expected, BaseUrl);
        }

        [Test]
        public void Executor_ReturnsNew()
        {
            var firstExecutor = Executor;
            var secondExecutor = Executor;

            Assert.AreNotEqual(firstExecutor, secondExecutor);
        }
    }

    public static class BaseApiTestsSource
    {
        public static readonly string TestHost = "venue-service.{0}tixuk.io/api/";

        public static IEnumerable<TestCaseData> BaseUrl_ReturnsCorrectly = new[]
        {
            new TestCaseData(
                new ApiContext(Environments.Production),
                "https://venue-service.tixuk.io/api/"
            ),
            new TestCaseData(
                new ApiContext(Environments.Sandbox),
                "https://venue-service.devtixuk.io/api/"
            ),
            new TestCaseData(
                new ApiContext(Environments.Staging),
                "https://venue-service.stagingtixuk.io/api/"
            ),
            new TestCaseData(
                new ApiContext(Environments.QA),
                "https://venue-service.qatixuk.io/api/"
            ),
        };
    }
}
