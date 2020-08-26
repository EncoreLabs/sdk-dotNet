using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Tests.Helpers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Results.Exceptions
{
    internal class ContextApiExceptionTests
    {
        [Test]
        public void ConstructorWithInfo_InitializesContextErrors()
        {
            var info = new List<Info>();

            var exception = new ContextApiException(info);

            Assert.AreEqual(info, exception.ContextErrors);
            Assert.Null(exception.Context);
            Assert.Null(exception.Response);
            Assert.Null(exception.RequestInResponse);
            Assert.Null(exception.ContextInResponse);
        }

        [Test]
        public void ConstructorWithInfoAndResponseArguments_InitializesContextErrorsAndResponseProperties()
        {
            var info = new List<Info>();
            var response = new RestResponse();
            var apiContext = new ApiContext();
            var context = new Context();
            var request = new Request();

            var exception = new ContextApiException(info, response, apiContext, context, request);

            Assert.AreEqual(info, exception.ContextErrors);
            Assert.AreEqual(apiContext, exception.Context);
            Assert.AreEqual(response, exception.Response);
            Assert.AreEqual(request, exception.RequestInResponse);
            Assert.AreEqual(context, exception.ContextInResponse);
        }

        [Test]
        public void ConstructorWithSourceException_InitializesContextErrorsAndResponseProprtiesBasedOnSourceException()
        {
            var info = new List<Info>();
            var response = new RestResponse();
            var apiContext = new ApiContext();
            var context = new Context();
            var request = new Request();
            var sourceException = new ContextApiException(info, response, apiContext, context, request);

            var exception = new ContextApiException(sourceException);

            Assert.AreEqual(sourceException.ContextErrors, exception.ContextErrors);
            Assert.AreEqual(sourceException.Context, exception.Context);
            Assert.AreEqual(sourceException.Response, exception.Response);
            Assert.AreEqual(sourceException.RequestInResponse, exception.RequestInResponse);
            Assert.AreEqual(sourceException.ContextInResponse, exception.ContextInResponse);
        }

        [TestCaseSource(typeof(ContextApiExceptionTestsSource), nameof(ContextApiExceptionTestsSource.Errors_ReturnsExpectedValue))]
        public void Errors_ReturnsExpectedValue(
            IEnumerable<Info> infosAsErrors,
            IRestResponse response,
            Context context,
            List<string> expected)
        {
            var exception = new ContextApiException(
                infosAsErrors,
                response,
                It.IsAny<ApiContext>(),
                context,
                It.IsAny<Request>());

            var actual = exception.Errors;

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(ContextApiExceptionTestsSource), nameof(ContextApiExceptionTestsSource.Message_IfErrorsDoesNotExist_ReturnsDefaultMessage))]
        public void Message_IfErrorsDoesNotExist_ReturnsDefaultMessage(
            IEnumerable<Info> infosAsErrors,
            IRestResponse response,
            Context context)
        {
            var exception = new ContextApiException(
                infosAsErrors,
                response,
                It.IsAny<ApiContext>(),
                context,
                It.IsAny<Request>());

            var actual = exception.Message;

            Assert.NotNull(actual);
        }

        [TestCaseSource(typeof(ContextApiExceptionTestsSource), nameof(ContextApiExceptionTestsSource.Message_IfErrorsExists_ReturnsExpectedValue))]
        public void Message_IfErrorsExists_ReturnsExpectedValue(
            IEnumerable<Info> infosAsErrors,
            IRestResponse response,
            Context context,
            string expected)
        {
            var exception = new ContextApiException(
                infosAsErrors,
                response,
                It.IsAny<ApiContext>(),
                context,
                It.IsAny<Request>());

            var actual = exception.Message;

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class ContextApiExceptionTestsSource
    {
        public static IEnumerable<TestCaseData> Errors_ReturnsExpectedValue { get; } = new[]
        {
            new TestCaseData(
                null,
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                null),
            new TestCaseData(
                new List<Info>(),
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                null),
            new TestCaseData(
                new List<Info>
                {
                    new Info(),
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                null),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Some info" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                new List<string> { "Some info" }),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Code = "validation_error" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                new List<string> { "validation_error" }),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Validation error", Code = "validation_error", Type = "information" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                new List<string> { "Validation error" }),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Some info" },
                    new Info { Code = "validation_error" },
                    new Info(),
                    new Info { Message = "Validation error", Code = "validation_error", Type = "information" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                new List<string> { "Some info", "validation_error", "Validation error" }),
            new TestCaseData(
                null,
                new RestResponse { StatusDescription = "OK" },
                new Context { Errors = new List<Error> { new Error { Message = "ERROR" } } },
                null),
        };

        public static IEnumerable<TestCaseData> Message_IfErrorsDoesNotExist_ReturnsDefaultMessage { get; } = new[]
        {
            new TestCaseData(
                null,
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>()),
            new TestCaseData(
                new List<Info>(),
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>()),
            new TestCaseData(
                new List<Info>
                {
                    new Info(),
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>()),
            new TestCaseData(
                null,
                new RestResponse { StatusDescription = "OK" },
                new Context { Errors = new List<Error> { new Error { Message = "ERROR" } } }),
        };

        public static IEnumerable<TestCaseData> Message_IfErrorsExists_ReturnsExpectedValue { get; } = new[]
        {
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Some info" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                "Some info"),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Code = "validation_error" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                "validation_error"),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Validation error", Code = "validation_error", Type = "information" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                "Validation error"),
            new TestCaseData(
                new List<Info>
                {
                    new Info { Message = "Some info" },
                    new Info { Code = "validation_error" },
                    new Info(),
                    new Info { Message = "Validation error", Code = "validation_error", Type = "information" },
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<Context>(),
                "Some info\r\nvalidation_error\r\nValidation error"),
        };
    }
}
