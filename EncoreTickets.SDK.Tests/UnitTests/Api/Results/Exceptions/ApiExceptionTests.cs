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
        public void ConstructorWithResponseArguments_InitializesResponseProperties(IRestResponse response,
            ApiContext apiContext, Context context, Request request)
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
            var exception = new ApiException(response, It.IsAny<ApiContext>(),
                It.IsAny<SDK.Api.Results.Response.Context>(), It.IsAny<Request>());

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
            IRestResponse response, SDK.Api.Results.Response.Context context)
        {
            var exception = new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var actual = exception.Message;

            Assert.NotNull(actual);
        }

        [TestCaseSource(typeof(ApiExceptionTestsSource), nameof(ApiExceptionTestsSource.Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue))]
        public void Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue(
            IRestResponse response, SDK.Api.Results.Response.Context context, string expected)
        {
            var exception = new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var actual = exception.Message;

            Assert.AreEqual(expected, actual);
        }
    }

    public static class ApiExceptionTestsSource
    {
        public static IEnumerable<TestCaseData> ConstructorWithResponseArguments_InitializesResponseProperties = new[]
        {
            new TestCaseData(new RestResponse(), new ApiContext(), new Context(), new Request()),
            new TestCaseData(null, new ApiContext(), new Context(), new Request()),
            new TestCaseData(new RestResponse(), null, new Context(), new Request()),
            new TestCaseData(new RestResponse(), new ApiContext(), null, new Request()),
            new TestCaseData(new RestResponse(), new ApiContext(), new Context(), null),
        };

        public static IEnumerable<TestCaseData> ConstructorWithSourceException_InitializesResponseProprtiesBasedOnSourceException = new[]
        {
            new TestCaseData(new ApiException(new RestResponse(), new ApiContext(), new SDK.Api.Results.Response.Context(), new Request())),
            new TestCaseData(new ApiException()),
        };

        public static IEnumerable<TestCaseData> ResponseCode_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                null,
                default(HttpStatusCode)
                ) {TestName = "ResponseCode_IfResponseIsNull_ReturnsDefault"},
            new TestCaseData(
                new RestResponse(),
                default(HttpStatusCode)
            ) {TestName = "ResponseCode_IfResponseWithoutStatusCode_ReturnsDefault"},
            new TestCaseData(
                new RestResponse{StatusCode = HttpStatusCode.InternalServerError},
                HttpStatusCode.InternalServerError
            ) {TestName = "ResponseCode_IfResponseWithStatusCode_ReturnsStatusCode"},
            new TestCaseData(
                new RestResponse{StatusCode = HttpStatusCode.OK},
                HttpStatusCode.OK
            ) {TestName = "ResponseCode_IfResponseWithStatusCode_ReturnsStatusCode"},
        };

        public static IEnumerable<TestCaseData> Errors_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                null,
                null,
                null
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseIsNull_ReturnsNull"},
            new TestCaseData(
                null,
                new SDK.Api.Results.Response.Context(),
                null
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseIsNull_ReturnsNull"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error"
                },
                null,
                new List<string>
                {
                    "Internal server error"
                }
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseStatusDescriptionExists_ReturnsResponseStatusDescription"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error"
                },
                new SDK.Api.Results.Response.Context(),
                new List<string>
                {
                    "Internal server error"
                }
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseStatusDescriptionExists_ReturnsResponseStatusDescription"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured"
                },
                null,
                new List<string>
                {
                    "Some error has occured"
                }
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseStatusDescriptionIsEmpty_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured"
                },
                new SDK.Api.Results.Response.Context(),
                new List<string>
                {
                    "Some error has occured"
                }
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseStatusDescriptionIsEmpty_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured"
                },
                null,
                new List<string>
                {
                    "Some error has occured"
                }
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured"
                },
                new SDK.Api.Results.Response.Context(),
                new List<string>
                {
                    "Some error has occured"
                }
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = ""
                },
                null,
                null
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsEmpty_ReturnsNull"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = ""
                },
                new SDK.Api.Results.Response.Context(),
                null
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsEmpty_ReturnsNull"},
            new TestCaseData(
                new RestResponse(),
                null,
                null
            ) {TestName = "Errors_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsNull_ReturnsNull"},
            new TestCaseData(
                new RestResponse(),
                new SDK.Api.Results.Response.Context(),
                null
            ) {TestName = "Errors_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsNull_ReturnsNull"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>()
                },
                null
            ) {TestName = "Errors_IfContextErrorsIsEmpty_ReturnsNull"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error()
                    }
                },
                null
            ) {TestName = "Errors_IfContextErrorsHasNotInitializedError_ReturnsNull"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "" }
                    }
                },
                null
            ) {TestName = "Errors_IfContextErrorsHasErrorOnlyWithEmptyMessage_ReturnsNull"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "Venue [9] is not found" }
                    }
                },
                new List<string>
                {
                    "Venue [9] is not found"
                }
            ) {TestName = "Errors_IfContextErrorsHasErrorOnlyWithNotEmptyMessage_ReturnsErrorMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error {Field = "coupon.code"}
                    }
                },
                new List<string>
                {
                    "coupon.code - "
                }
            ) {TestName = "Errors_IfContextErrorsHasErrorOnlyWithNotEmptyField_ReturnsMessageWithField"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error", Field = "coupon.code"
                        }
                    }
                },
                new List<string>
                {
                    "coupon.code - This value should not be blank."
                }
            ) {TestName = "Errors_IfContextErrorsHasFullyInitializedError_ReturnsMessageBasedOnErrorMessageAndField"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "Product [9] is not found"
                        },
                        new Error
                        {
                            Message = "Unauthorized"
                        },
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error", Field = "coupon.code"
                        }
                    }
                },
                new List<string>
                {
                    "Product [9] is not found",
                    "Unauthorized",
                    "coupon.code - This value should not be blank."
                }
            ) {TestName = "Errors_IfContextErrorsHasErrors_ReturnsMessages"},
        };

        public static IEnumerable<TestCaseData> Message_IfPredefinedMessageIsNull_IfErrorsDoesNotExist_ReturnsDefaultMessage = new[]
        {
            new TestCaseData(
                null,
                null
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseIsNull_ReturnsDefaultMessage"},
            new TestCaseData(
                null,
                new SDK.Api.Results.Response.Context()
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseIsNull_ReturnsDefaultMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = ""
                },
                null
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsEmpty_ReturnsDefaultMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = ""
                },
                new SDK.Api.Results.Response.Context()
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsEmpty_ReturnsDefaultMessage"},
            new TestCaseData(
                new RestResponse(),
                null
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsNull_ReturnsDefaultMessage"},
            new TestCaseData(
                new RestResponse(),
                new SDK.Api.Results.Response.Context()
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageIsNull_ReturnsDefaultMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>()
                }
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsEmpty_ReturnsDefaultMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error()
                    }
                }
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasNotInitializedError_ReturnsDefaultMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "" }
                    }
                }
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasErrorOnlyWithEmptyMessage_ReturnsDefaultMessage"},
        };

        public static IEnumerable<TestCaseData> Message_IfPredefinedMessageIsNull_IfErrorsExists_ReturnsExpectedValue = new[]
        {
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error"
                },
                null,
                "Internal server error"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseStatusDescriptionExists_ReturnsResponseStatusDescription"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "Internal server error"
                },
                new SDK.Api.Results.Response.Context(),
                "Internal server error"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseStatusDescriptionExists_ReturnsResponseStatusDescription"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured"
                },
                null,
                "Some error has occured"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseStatusDescriptionIsEmpty_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    StatusDescription = "",
                    ErrorMessage = "Some error has occured"
                },
                new SDK.Api.Results.Response.Context(),
                "Some error has occured"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseStatusDescriptionIsEmpty_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured"
                },
                null,
                "Some error has occured"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextInResponseIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                new RestResponse
                {
                    ErrorMessage = "Some error has occured"
                },
                new SDK.Api.Results.Response.Context(),
                "Some error has occured"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsIsNull_IfResponseStatusDescriptionIsNull_IfResponseErrorMessageExists_ReturnsResponseErrorMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error { Message = "Venue [9] is not found" }
                    }
                },
                "Venue [9] is not found"
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasErrorOnlyWithNotEmptyMessage_ReturnsErrorMessage"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error {Field = "coupon.code"}
                    }
                },
                "coupon.code - "
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasErrorOnlyWithNotEmptyField_ReturnsMessageWithField"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error", Field = "coupon.code"
                        }
                    }
                },
                "coupon.code - This value should not be blank."
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasFullyInitializedError_ReturnsMessageBasedOnErrorMessageAndField"},
            new TestCaseData(
                It.IsAny<IRestResponse>(),
                new SDK.Api.Results.Response.Context
                {
                    Errors = new List<Error>
                    {
                        new Error
                        {
                            Message = "Product [9] is not found"
                        },
                        new Error
                        {
                            Message = "Unauthorized"
                        },
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error", Field = "coupon.code"
                        }
                    }
                },
                "Product [9] is not found; Unauthorized; coupon.code - This value should not be blank."
            ) {TestName = "Message_IfPredefinedMessageIsNull_IfContextErrorsHasErrors_ReturnsMessages"},
        };
    }
}
