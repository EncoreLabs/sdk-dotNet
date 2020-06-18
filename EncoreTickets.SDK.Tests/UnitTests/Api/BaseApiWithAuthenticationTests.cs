using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Authentication.JWTServices;
using EncoreTickets.SDK.Tests.Helpers.ApiWrappers;
using EncoreTickets.SDK.Utilities.Enums;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api
{
    internal class BaseApiWithAuthenticationTests : BaseApiWithAuthentication
    {
        public override int? ApiVersion { get; }

        public BaseApiWithAuthenticationTests()
            : base(new ApiContext(Environments.Sandbox), BaseApiWithAuthenticationTestsSource.TestHost)
        {
        }

        [Test]
        public void Constructor_IfWithAutomaticAuthentication_InitializesBaseParameters()
        {
            var context = new ApiContext();
            var host = "some_host";
            var automaticAuthentication = true;

            var actual = new BaseApiWithAuthenticationWrapper(context, host, automaticAuthentication);

            Assert.AreEqual(automaticAuthentication, actual.SourceAutomaticAuthentication);
            Assert.AreEqual(context, actual.SourceContext);
            Assert.AreEqual(host, actual.SourceHost);
        }

        [Test]
        public void Constructor_IfWithoutAutomaticAuthentication_InitializesBaseParametersAndAutomaticAuthIsFalse()
        {
            var context = new ApiContext();
            var host = "some_host";

            var actual = new BaseApiWithAuthenticationWrapper(context, host);

            Assert.IsFalse(actual.SourceAutomaticAuthentication);
            Assert.AreEqual(context, actual.SourceContext);
            Assert.AreEqual(host, actual.SourceHost);
        }

        [Test]
        public void AuthenticationService_ReturnsForCurrentContext()
        {
            Context = new ApiContext
            {
                AuthenticationMethod = AuthenticationMethod.Basic,
            };
            var basicService = AuthenticationService;
            Context = new ApiContext
            {
                AuthenticationMethod = AuthenticationMethod.JWT,
            };
            var jwtService = AuthenticationService;

            Assert.AreNotEqual(basicService, jwtService);
        }

        [TestCaseSource(
            typeof(BaseApiWithAuthenticationTestsSource),
            nameof(BaseApiWithAuthenticationTestsSource.GetAuthenticationService_IfCorrespondingAuthServiceExists_ReturnsCorrectAuthenticationService))]
        public void GetAuthenticationService_IfCorrespondingAuthServiceExists_ReturnsCorrectAuthenticationService(ApiContext context, Type expectedType)
        {
            var actual = GetAuthenticationService(context);

            Assert.IsInstanceOf(expectedType, actual);
        }

        [TestCase(AuthenticationMethod.Basic)]
        [TestCase((AuthenticationMethod)1090)]
        public void GetAuthenticationService_IfCorrespondingAuthServiceDoesNotExist_ReturnsCorrectAuthenticationService(AuthenticationMethod authenticationMethod)
        {
            var context = new ApiContext
            {
                AuthenticationMethod = authenticationMethod,
            };

            var actual = GetAuthenticationService(context);

            Assert.IsNull(actual);
        }
    }

    internal class BaseApiWithAuthenticationTestsWithMockers : BaseApiWithAuthentication
    {
        private readonly Mock<IAuthenticationService> authenticationServiceMocker;

        public override int? ApiVersion { get; }

        public override IAuthenticationService AuthenticationService => authenticationServiceMocker.Object;

        public BaseApiWithAuthenticationTestsWithMockers()
            : base(new ApiContext(Environments.Sandbox), BaseApiWithAuthenticationTestsSource.TestHost)
        {
            authenticationServiceMocker = new Mock<IAuthenticationService>();
        }

        [Test]
        public void TriggerAutomaticAuthentication_IfAutomaticAuthenticationTrue_CallsAuthentication()
        {
            AutomaticAuthentication = true;
            authenticationServiceMocker.Setup(x => x.Authenticate()).Callback(() => { });

            TriggerAutomaticAuthentication();

            authenticationServiceMocker.Verify(x => x.Authenticate(), Times.Once);
        }

        [Test]
        public void TriggerAutomaticAuthentication_IfAutomaticAuthenticationFalse_DoesNotCallAuthentication()
        {
            AutomaticAuthentication = false;
            authenticationServiceMocker.Setup(x => x.Authenticate()).Callback(() => { });

            TriggerAutomaticAuthentication();

            authenticationServiceMocker.Verify(x => x.Authenticate(), Times.Never);
        }
    }

    internal static class BaseApiWithAuthenticationTestsSource
    {
        public static readonly string TestHost = "venue-service.{0}tixuk.io/api/";

        public static IEnumerable<TestCaseData> GetAuthenticationService_IfCorrespondingAuthServiceExists_ReturnsCorrectAuthenticationService { get; } = new[]
        {
            new TestCaseData(
                new ApiContext(Environments.Production),
                typeof(PredefinedJwtAuthenticationService)),
            new TestCaseData(
                new ApiContext(),
                typeof(PredefinedJwtAuthenticationService)),
            new TestCaseData(
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.PredefinedJWT,
                },
                typeof(PredefinedJwtAuthenticationService)),
            new TestCaseData(
                new ApiContext
                {
                    AuthenticationMethod = AuthenticationMethod.JWT,
                },
                typeof(JwtAuthenticationService)),
        };
    }
}
