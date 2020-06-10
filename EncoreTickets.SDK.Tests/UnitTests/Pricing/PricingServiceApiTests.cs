using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Pricing;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Pricing
{
    internal class PricingServiceApiTests : PricingServiceApi
    {
        private MockersForApiServiceWithAuthentication mockers;

        public override IAuthenticationService AuthenticationService => mockers.AuthenticationServiceMock.Object;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public PricingServiceApiTests() : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiServiceWithAuthentication();
        }

        [Test]
        public void GetPriceBands_NullProductId_ArgumentException()
        {
            Assert.Catch<ArgumentException>(() => { GetPriceBands(null, 2, DateTime.Now); });
        }
    }
}