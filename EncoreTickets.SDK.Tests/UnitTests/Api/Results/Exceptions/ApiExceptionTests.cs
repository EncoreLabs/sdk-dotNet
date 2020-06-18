using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Tests.Helpers;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Results.Exceptions
{
    internal class ApiExceptionTests
    {
        [Test]
        public void ConstructorWithNothing_DoesNotInitializeResponseProperties()
        {
            var exception = new ApiException();

            Assert.Null(exception.Context);
            Assert.Null(exception.Response);
            Assert.Null(exception.RequestInResponse);
            Assert.Null(exception.ContextInResponse);
        }

        [Test]
        public void ConstructorWithMessage_DoesNotInitializeResponseProperties()
        {
            var response = new RestResponse();
            var context = new ApiContext();
            var exception = new ApiException(It.IsAny<string>(), response, context);

            Assert.AreEqual(context, exception.Context);
            Assert.AreEqual(response, exception.Response);
            Assert.Null(exception.RequestInResponse);
            Assert.Null(exception.ContextInResponse);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.ConstructorWithResponseArguments_InitializesResponseProperties))]
        public void ConstructorWithResponseArguments_InitializesResponseProperties(
            IRestResponse response,
            ApiContext apiContext,
            Context context,
            Request request)
        {
            var exception = new ApiException(response, apiContext, context, request);

            Assert.AreEqual(apiContext, exception.Context);
            Assert.AreEqual(response, exception.Response);
            Assert.AreEqual(request, exception.RequestInResponse);
            Assert.AreEqual(context, exception.ContextInResponse);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.ConstructorWithSourceException_InitializesResponseProprtiesBasedOnSourceException))]
        public void ConstructorWithSourceException_InitializesResponseProprtiesBasedOnSourceException(
            ApiException sourceException)
        {
            var exception = new ApiException(sourceException);

            Assert.AreEqual(sourceException.Context, exception.Context);
            Assert.AreEqual(sourceException.Response, exception.Response);
            Assert.AreEqual(sourceException.RequestInResponse, exception.RequestInResponse);
            Assert.AreEqual(sourceException.ContextInResponse, exception.ContextInResponse);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.ResponseCode_ReturnsExpectedValue))]
        public void ResponseCode_ReturnsExpectedValue(IRestResponse response, HttpStatusCode expected)
        {
            var exception = new ApiException(
                response,
                It.IsAny<ApiContext>(),
                It.IsAny<Context>(),
                It.IsAny<Request>());

            var actual = exception.ResponseCode;

            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.Errors_ReturnsExpectedValue))]
        public void Errors_ReturnsExpectedValue(IRestResponse response, Context context, List<string> expected)
        {
            var exception = new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var actual = exception.Errors;

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCase("Some error has occured")]
        [TestCase("Test message")]
        public void Message_IfPredefinedMessageExists_ReturnsPredefinedMessage(string predefinedMessage)
        {
            var exception = new ApiException(predefinedMessage, It.IsAny<IRestResponse>(), It.IsAny<ApiContext>());

            var actual = exception.Message;

            Assert.AreEqual(predefinedMessage, actual);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.Message_IfPredefinedMessageIsNull_IfErrorsDoesNotExist_ReturnsDefaultMessage))]
        public void Message_IfPredefinedMessageIsNull_IfErrorsDoesNotExist_ReturnsDefaultMessage(
            IRestResponse response, Context context)
        {
            var exception = new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var actual = exception.Message;

            Assert.NotNull(actual);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue))]
        public void Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue(
            IRestResponse response, Context context, string expected)
        {
            var exception = new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var actual = exception.Message;

            Assert.AreEqual(expected, actual);
        }
    }

    internal static class ApiExceptionTestsSource
    {
        public static IEnumerable<TestCaseData> ConstructorWithResponseArguments_InitializesResponseProperties { get; } = new[]
            {
                new TestCaseData(new RestResponse(), new ApiContext(), new Context(), new Request()),
                new TestCaseData(null, new ApiContext(), new Context(), new Request()),
                new TestCaseData(new RestResponse(), null, new Context(), new Request()),
                new TestCaseData(new RestResponse(), new ApiContext(), null, new Request()),
                new TestCaseData(new RestResponse(), new ApiContext(), new Context(), null),
            };

        public static IEnumerable<TestCaseData> ConstructorWithSourceException_InitializesResponseProprtiesBasedOnSourceException { get; } = new[]
        {
            new TestCaseData(new ApiException(new RestResponse(), new ApiContext(), new Context(), new Request())),
            new TestCaseData(new ApiException()),
        };

        public static IEnumerable<TestCaseData> ResponseCode_ReturnsExpectedValue { get; } = new[]
        {
            new TestCaseData(
                null,
                default(HttpStatusCode)),
            new TestCaseData(
                new RestResponse(),
                default(HttpStatusCode)),
            new TestCaseData(
                new RestResponse { StatusCode = HttpStatusCode.InternalServerError },
                HttpStatusCode.InternalServerError),
            new TestCaseData(
                new RestResponse { StatusCode = HttpStatusCode.OK },
                HttpStatusCode.OK),
        };

        public static IEnumerable<TestCaseData> Errors_ReturnsExpectedValue { get; } = new[]
        {
            new TestCaseData(
                null,
                null,
                null),
            new TestCaseData(
                null,
                new Context(),
                null),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error",
                },
                null,
                new List<string>
                {
                    "Internal server error",
                }),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error",
                },
                new Context(),
                new List<string>
                {
                    "Internal server error",
                }),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured",
                },
                null,
                new List<string>
                {
                    "Some error has occured",
                }),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured",
                },
                new Context(),
                new List<string>
                {
                    "Some error has occured",
                }),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured",
                },
                null,
                new List<string>
                {
                    "Some error has occured",
                }),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured",
                },
                new Context(),
                new List<string>
                {
                    "Some error has occured",
                }),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "",
                },
                null,
                null),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "",
                },
                new Context(),
                null),
            new TestCaseData(
                new RestResponse(),
                null,
                null),
            new TestCaseData(
                new RestResponse(),
                new Context(),
                null),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>(),
                },
                null),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error(),
                    },
                },
                null),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "" },
                    },
                },
                null),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "Venue [9] is not found" },
                    },
                },
                new List<string>
                {
                    "Venue [9] is not found",
                }),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Field = "coupon.code" },
                    },
                },
                new List<string>
                {
                    "coupon.code: this field is invalid",
                }),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code",
                        },
                    },
                },
                new List<string>
                {
                    "coupon.code: This value should not be blank.",
                }),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "Product [9] is not found",
                        },
                        new Error
                        {
                            Message = "Unauthorized",
                        },
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code",
                        },
                    },
                },
                new List<string>
                {
                    "Product [9] is not found",
                    "Unauthorized",
                    "coupon.code: This value should not be blank.",
                }),
        };

        public static IEnumerable<TestCaseData> Message_IfPredefinedMessageIsNull_IfErrorsDoesNotExist_ReturnsDefaultMessage { get; } = new[]
        {
            new TestCaseData(
                null,
                null),
            new TestCaseData(
                null,
                new Context()),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "",
                },
                null),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "",
                },
                new Context()),
            new TestCaseData(
                new RestResponse(),
                null),
            new TestCaseData(
                new RestResponse(),
                new Context()),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>(),
                }),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error(),
                    },
                }),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "" },
                    },
                }),
        };

        public static IEnumerable<TestCaseData> Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue { get; } = new[]
        {
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error",
                },
                null,
                "Internal server error"),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error",
                },
                new Context(),
                "Internal server error"),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured",
                },
                null,
                "Some error has occured"),
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured",
                },
                new Context(),
                "Some error has occured"),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured",
                },
                null,
                "Some error has occured"),
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured",
                },
                new Context(),
                "Some error has occured"),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "Venue [9] is not found" },
                    },
                },
                "Venue [9] is not found"),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Field = "coupon.code" },
                    },
                },
                "coupon.code: this field is invalid"),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code",
                        },
                    },
                },
                "coupon.code: This value should not be blank."),
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "Product [9] is not found",
                        },
                        new Error
                        {
                            Message = "Unauthorized",
                        },
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code",
                        },
                    },
                },
                "Product [9] is not found\r\nUnauthorized\r\ncoupon.code: This value should not be blank."),
        };
    }
}
