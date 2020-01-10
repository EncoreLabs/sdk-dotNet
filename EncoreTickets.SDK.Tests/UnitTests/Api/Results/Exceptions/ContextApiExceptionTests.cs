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
            var context = new SDK.Api.Results.Response.Context();
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
            var context = new SDK.Api.Results.Response.Context();
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
        public void Errors_ReturnsExpectedValue(IEnumerable<Info> infosAsErrors, IRestResponse response,
            SDK.Api.Results.Response.Context context, List<string> expected)
        {
            var exception = new ContextApiException(infosAsErrors, response, It.IsAny<ApiContext>(), context,
                It.IsAny<Request>());

            var actual = exception.Errors;

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(ContextApiExceptionTestsSource), nameof(ContextApiExceptionTestsSource.Message_IfErrorsDoesNotExist_ReturnsDefaultMessage))]
        public void Message_IfErrorsDoesNotExist_ReturnsDefaultMessage(IEnumerable<Info> infosAsErrors, IRestResponse response,
            SDK.Api.Results.Response.Context context)
        {
            var exception = new ContextApiException(infosAsErrors, response, It.IsAny<ApiContext>(), context,
                It.IsAny<Request>());

            var actual = exception.Message;

            Assert.NotNull(actual);
        }

        [TestCaseSource(typeof(ContextApiExceptionTestsSource), nameof(ContextApiExceptionTestsSource.Message_IfErrorsExists_ReturnsExpectedValue))]
        public void Message_IfErrorsExists_ReturnsExpectedValue(IEnumerable<Info> infosAsErrors, IRestResponse response,
            SDK.Api.Results.Response.Context context, string expected)
        {
            var exception = new ContextApiException(infosAsErrors, response, It.IsAny<ApiContext>(), context,
                It.IsAny<Request>());

            var actual = exception.Message;

            Assert.AreEqual(expected, actual);
        }
    }

    public static class ContextApiExceptionTestsSource
    {
        public static IEnumerable<TestCaseData> Errors_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                null,
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                null
            ) {TestName = "Errors_IfContextErrorsIsNull_ReturnsNull"},
            new TestCaseData(
                new List<Info>(),
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                null
            ) {TestName = "Errors_IfContextErrorsIsEmpty_ReturnsNull"},
            new TestCaseData(
                new List<Info>
                {
                    new Info()
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                null
            ) {TestName = "Errors_IfContextErrorsHasNotInitializedInfo_ReturnsNull"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Some info"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                new List<string> {"Some info"}
            ) {TestName = "Errors_IfContextErrorsHasInfoWithMessage_ReturnsInfoMessage"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Code = "validation_error"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                new List<string> {"validation_error"}
            ) {TestName = "Errors_IfContextErrorsHasInfoOnlyWithCode_ReturnsInfoCode"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Validation error", Code = "validation_error", Type = "information"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                new List<string> {"Validation error"}
            ) {TestName = "Errors_IfContextErrorsHasFullyInitializedInfo_ReturnsInfoMessage"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Some info"},
                    new Info {Code = "validation_error"},
                    new Info(),
                    new Info {Message = "Validation error", Code = "validation_error", Type = "information"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                new List<string> {"Some info", "validation_error", "Validation error"}
            ) {TestName = "Errors_IfContextErrorsHasInfos_ReturnsInfoMessages"},
            new TestCaseData(
                null,
                new RestResponse {StatusDescription = "OK"},
                new SDK.Api.Results.Response.Context {Errors = new List<Error> {new Error {Message = "ERROR"}}},
                null
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseArgumentsWereSet_ReturnsNull"},
        };

        public static IEnumerable<TestCaseData> Message_IfErrorsDoesNotExist_ReturnsDefaultMessage = new[]
        {
            new TestCaseData(
                null,
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>()
            ) {TestName = "Message_IfErrorsDoesNotExist_IfContextErrorsIsNull_ReturnsDefaultMessage"},
            new TestCaseData(
                new List<Info>(),
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>()
            ) {TestName = "Message_IfErrorsDoesNotExist_IfContextErrorsIsEmpty_ReturnsDefaultMessage"},
            new TestCaseData(
                new List<Info>
                {
                    new Info()
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>()
            ) {TestName = "Message_IfErrorsDoesNotExist_IfContextErrorsHasNotInitializedInfo_ReturnsDefaultMessage"},
            new TestCaseData(
                null,
                new RestResponse {StatusDescription = "OK"},
                new SDK.Api.Results.Response.Context {Errors = new List<Error> {new Error {Message = "ERROR"}}}
            ) {TestName = "Message_IfErrorsDoesNotExist_IfContextErrorsIsNull_IfResponseArgumentsWereSet_ReturnsDefaultMessage"},
        };

        public static IEnumerable<TestCaseData> Message_IfErrorsExists_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Some info"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                "Some info"
            ) {TestName = "Message_IfErrorsExists__IfContextErrorsHasInfoWithMessage_ReturnsInfoMessage"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Code = "validation_error"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                "validation_error"
            ) {TestName = "Message_IfErrorsExists__IfContextErrorsHasInfoOnlyWithCode_ReturnsInfoCode"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Validation error", Code = "validation_error", Type = "information"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                "Validation error"
            ) {TestName = "Message_IfErrorsExists__IfContextErrorsHasFullyInitializedInfo_ReturnsInfoMessage"},
            new TestCaseData(
                new List<Info>
                {
                    new Info {Message = "Some info"},
                    new Info {Code = "validation_error"},
                    new Info(),
                    new Info {Message = "Validation error", Code = "validation_error", Type = "information"}
                },
                It.IsAny<IRestResponse>(),
                It.IsAny<SDK.Api.Results.Response.Context>(),
                "Some info; validation_error; Validation error"
            ) {TestName = "Message_IfErrorsExists__IfContextErrorsHasInfos_ReturnsInfoMessages"},
        };
    }
}
