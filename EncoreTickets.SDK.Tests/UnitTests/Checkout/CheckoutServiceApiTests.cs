using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Checkout;
using EncoreTickets.SDK.Checkout.Models;
using EncoreTickets.SDK.Checkout.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using EncoreTickets.SDK.Utilities.Encoders;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Checkout
{
    internal class CheckoutServiceApiTests : CheckoutServiceApi
    {
        private static readonly BookingParameters TestValidBookingParameters = new BookingParameters();

        private static readonly string TestValidBookingReference = "some_id";

        private static readonly ConfirmBookingParameters TestValidConfirmParameters = new ConfirmBookingParameters();

        private MockersForApiService mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public CheckoutServiceApiTests() : base(ApiContextTestHelper.DefaultApiContext)
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiService();
            ApiContextTestHelper.ResetContextToDefault(Context);
        }

        #region Checkout

        [Test]
        public void Checkout_IfBookingIsNotSet_ThrowsArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                Checkout(It.IsAny<BookingParameters>());
            });
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.Checkout_IfBookingIsSet_CallsApiWithRightParameters))]
        public void Checkout_IfBookingIsSet_CallsApiWithRightParameters(BookingParameters parameters, string requestBody)
        {
            mockers.SetupAnyExecution<ApiResponse<PaymentInfo>>();

            try
            {
                Checkout(parameters);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<PaymentInfo>>(
                BaseUrl,
                $"v{ApiVersion}/checkout",
                Method.POST,
                bodyInJson: requestBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.Checkout_IfApiResponseSuccessful_ReturnsPaymentInfo))]
        public void Checkout_IfApiResponseSuccessful_ReturnsPaymentInfo(
            string responseContent,
            PaymentInfo expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<PaymentInfo>>(responseContent);

            var actual = Checkout(TestValidBookingParameters);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.Checkout_IfApiResponseFailed_ThrowsApiException))]
        public void Checkout_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<PaymentInfo>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = Checkout(TestValidBookingParameters);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region ConfirmBooking

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("     ")]
        public void ConfirmBooking_IfBookingReferenceIsNotSet_ThrowsArgumentException(string reference)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ConfirmBooking(reference, It.IsAny<ConfirmBookingParameters>());
            });
        }

        [Test]
        public void ConfirmBooking_IfParametersAreNotSet_ThrowsArgumentException()
        {
            Assert.Catch<ArgumentException>(() =>
            {
                ConfirmBooking(TestValidBookingReference, It.IsAny<ConfirmBookingParameters>());
            });
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource),
            nameof(CheckoutServiceApiTestsSource.ConfirmBooking_IfParametersAreSet_IfNotAgentBooking_CallsApiWithRightParameters))]
        public void ConfirmBooking_IfParametersAreSet_IfNotAgentBooking_CallsApiWithRightParameters(string reference,
            ConfirmBookingParameters parameters, string requestBody)
        {
            mockers.SetupAnyExecution<ApiResponseWithResultsBlock<string>>();

            try
            {
                ConfirmBooking(reference, parameters);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponseWithResultsBlock<string>>(
                BaseUrl,
                $"v{ApiVersion}/bookings/{reference}/confirm",
                Method.POST,
                bodyInJson: requestBody,
                expectedHeaders: null,
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.ConfirmBooking_IfParametersAreSet_IfAgentBooking_CallsApiWithRightParameters))]
        public void ConfirmBooking_IfParametersAreSet_IfAgentBooking_CallsApiWithRightParameters(string agentId, string agentPassword, 
            string reference, ConfirmBookingParameters parameters, string requestBody)
        {
            var encoder = new Base64Encoder();
            mockers.SetupAnyExecution<ApiResponseWithResultsBlock<string>>();

            try
            {
                ConfirmBooking(agentId, agentPassword, reference, parameters);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponseWithResultsBlock<string>>(
                BaseUrl,
                $"v{ApiVersion}/bookings/{reference}/confirm",
                Method.POST,
                bodyInJson: requestBody,
                expectedHeaders: new Dictionary<string, object>
                {
                    {"X-AGENT-ID", encoder.Encode(agentId)},
                    {"X-AGENT-PASSWORD", encoder.Encode(agentPassword)}
                },
                expectedQueryParameters: null);
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.ConfirmBooking_IfApiResponseSuccessful_ReturnsBoolResult))]
        public void ConfirmBooking_IfApiResponseSuccessful_ReturnsBoolResult(string responseContent)
        {
            mockers.SetupSuccessfulExecution<ApiResponseWithResultsBlock<string>>(responseContent);

            var actual = ConfirmBooking(TestValidBookingReference, TestValidConfirmParameters);

            Assert.IsTrue(actual);
        }

        [TestCaseSource(typeof(CheckoutServiceApiTestsSource), nameof(CheckoutServiceApiTestsSource.ConfirmBooking_IfApiResponseFailed_ThrowsApiException))]
        public void ConfirmBooking_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponseWithResultsBlock<string>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = ConfirmBooking(TestValidBookingReference, TestValidConfirmParameters);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion
    }

    public static class CheckoutServiceApiTestsSource
    {
        #region Checkout

        public static IEnumerable<TestCaseData> Checkout_IfBookingIsSet_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                new BookingParameters
                {
                    Reference = "8527089",
                    ChannelId = "europa-test",
                    Shopper = new Shopper
                    {
                        Email = "user@example.com",
                        Title = "Miss",
                        FirstName = "Shawn",
                        LastName = "Butler",
                        TelephoneNumber = "07882571812",
                        ExternalId = "8263702"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Barnard's Inn",
                        Line2 = "86 Fetter Lane",
                        PostalCode = "EC4A 1EN",
                        City = "London",
                        CountryCode = "GB",
                        CountryName = "United Kingdom",
                        StateOrProvince = "NY"
                    },
                    Origin = "https://example.com",
                    RedirectUrl = "https://example.com",
                    DeliveryMethod = DeliveryMethod.Collection,
                    DeliveryCharge = 245,
                    RecipientName = "Mr. Someone Else",
                    GiftVoucherMessage = "Happy Birthday to you.",
                    DeliveryAddress = new Address
                    {
                        Line1 = "Barnard's Inn",
                        Line2 = "86 Fetter Lane",
                        PostalCode = "EC4A 1EN",
                        City = "London",
                        CountryCode = "GB",
                        CountryName = "United Kingdom",
                        StateOrProvince = "NY"
                    },
                    HasFlexiTickets = true,
                    PaymentType = PaymentType.Card
                },
                "{\"deliveryMethod\":\"C\",\"paymentType\":\"card\",\"reference\":\"8527089\",\"channelId\":\"europa-test\",\"shopper\":{\"email\":\"user@example.com\",\"title\":\"Miss\",\"firstName\":\"Shawn\",\"lastName\":\"Butler\",\"telephoneNumber\":\"07882571812\",\"externalId\":\"8263702\"},\"billingAddress\":{\"line1\":\"Barnard's Inn\",\"line2\":\"86 Fetter Lane\",\"postalCode\":\"EC4A 1EN\",\"city\":\"London\",\"countryCode\":\"GB\",\"countryName\":\"United Kingdom\",\"stateOrProvince\":\"NY\"},\"origin\":\"https://example.com\",\"redirectUrl\":\"https://example.com\",\"deliveryCharge\":245,\"recipientName\":\"Mr. Someone Else\",\"giftVoucherMessage\":\"Happy Birthday to you.\",\"deliveryAddress\":{\"line1\":\"Barnard's Inn\",\"line2\":\"86 Fetter Lane\",\"postalCode\":\"EC4A 1EN\",\"city\":\"London\",\"countryCode\":\"GB\",\"countryName\":\"United Kingdom\",\"stateOrProvince\":\"NY\"},\"hasFlexiTickets\":true,\"paymentId\":null}"
            ),
            new TestCaseData(
                new BookingParameters
                {
                    Reference = "8602898",
                    ChannelId = "resia",
                    Shopper = new Shopper
                    {
                        Email = "agentEmail@mail.com",
                        Title = "Mrs",
                        FirstName = "clientFName",
                        LastName = "clientLName",
                        TelephoneNumber = "123321321321"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "47-51 Great Suffolk St",
                        Line2 = "",
                        PostalCode = "SE1 0BS",
                        City = "London",
                        CountryCode = "UK"
                    },
                    RedirectUrl = "http://localhost:8000/",
                    DeliveryMethod = DeliveryMethod.Collection,
                    PaymentType = PaymentType.Account,
                    PaymentId = "111"
                },
                "{\"deliveryMethod\":\"C\",\"paymentType\":\"account\",\"reference\":\"8602898\",\"channelId\":\"resia\",\"shopper\":{\"email\":\"agentEmail@mail.com\",\"title\":\"Mrs\",\"firstName\":\"clientFName\",\"lastName\":\"clientLName\",\"telephoneNumber\":\"123321321321\",\"externalId\":null},\"billingAddress\":{\"line1\":\"47-51 Great Suffolk St\",\"line2\":\"\",\"postalCode\":\"SE1 0BS\",\"city\":\"London\",\"countryCode\":\"UK\",\"countryName\":null,\"stateOrProvince\":null},\"origin\":null,\"redirectUrl\":\"http://localhost:8000/\",\"deliveryCharge\":0,\"recipientName\":null,\"giftVoucherMessage\":null,\"deliveryAddress\":null,\"hasFlexiTickets\":false,\"paymentId\":\"111\"}"
            ),
        };
        
        public static IEnumerable<TestCaseData> Checkout_IfApiResponseSuccessful_ReturnsPaymentInfo = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n\t\""paymentId\"": \""111\"",\r\n\t\""billingAddress\"": {\r\n\t\t\""city\"": \""Y Barri\"",\r\n\t\t\""countryCode\"": \""UK\"",\r\n\t\t\""line1\"": \""55-55A Fontygary Road\"",\r\n\t\t\""line2\"": \""Y Rhws\"",\r\n\t\t\""postalCode\"": \""CF62 3DT\""\r\n\t},\r\n\t\""channelId\"": \""europa-test\"",\r\n\t\""redirectUrl\"": \""http://localhost:8000/\"",\r\n\t\""deliveryMethod\"": \""C\"",\r\n\t\""reference\"": \""8526754\"",\r\n\t\""shopper\"": {\r\n\t\t\""email\"": \""artyonn@gmail.com\"",\r\n\t\t\""firstName\"": \""?><|†◊♠♣♥♦\"",\r\n\t\t\""lastName\"": \""test\"",\r\n\t\t\""telephoneNumber\"": \""123321321321\"",\r\n\t\t\""title\"": \""Mr\""\r\n\t}\r\n}""
    },
    ""response"": {
        ""paymentId"": ""c72b2b7c-69ef-489c-83ee-1c731af93324"",
        ""paymentType"": null
    }
}",
                new PaymentInfo
                {
                    PaymentId = "c72b2b7c-69ef-489c-83ee-1c731af93324",
                    PaymentType = null
                }
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n\t\""paymentId\"": \""111\"",\r\n\t\""billingAddress\"": {\r\n\t\t\""city\"": \""Y Barri\"",\r\n\t\t\""countryCode\"": \""UK\"",\r\n\t\t\""line1\"": \""55-55A Fontygary Road\"",\r\n\t\t\""line2\"": \""Y Rhws\"",\r\n\t\t\""postalCode\"": \""CF62 3DT\""\r\n\t},\r\n\t\""channelId\"": \""europa-test\"",\r\n\t\""redirectUrl\"": \""http://localhost:8000/\"",\r\n\t\""deliveryMethod\"": \""C\"",\r\n\t\""reference\"": \""8526754\"",\r\n\t\""shopper\"": {\r\n\t\t\""email\"": \""artyonn@gmail.com\"",\r\n\t\t\""firstName\"": \""?><|†◊♠♣♥♦\"",\r\n\t\t\""lastName\"": \""test\"",\r\n\t\t\""telephoneNumber\"": \""123321321321\"",\r\n\t\t\""title\"": \""Mr\""\r\n\t}\r\n}""
    },
    ""response"": {
        ""paymentId"": ""c72b2b7c-69ef-489c-83ee-1c731af93324"",
        ""paymentType"": ""card""
    }
}",
                new PaymentInfo
                {
                    PaymentId = "c72b2b7c-69ef-489c-83ee-1c731af93324",
                    PaymentType = PaymentType.Card
                }
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\n\t\""paymentId\"": \""111\"",\n\t\""paymentType\"": \""account\"",\n\t\""billingAddress\"": {\n\t\t\""city\"": \""London\"",\n\t\t\""countryCode\"": \""UK\"",\n\t\t\""line1\"": \""47-51 Great Suffolk St\"",\n\t\t\""line2\"": \""\"",\n\t\t\""postalCode\"": \""SE1 0BS\""\n\t},\n\t\""channelId\"": \""resiaapi\"",\n\t\""redirectUrl\"": \""http://localhost:8000/\"",\n\t\""deliveryMethod\"": \""C\"",\n\t\""reference\"": \""8527512\"",\n\t\""shopper\"": {\n\t\t\""email\"": \""agentEmail@mail.com\"",\n\t\t\""firstName\"": \""clientFName\"",\n\t\t\""lastName\"": \""clientLName\"",\n\t\t\""telephoneNumber\"": \""123321321321\"",\n\t\t\""title\"": \""Mrs\""\n\t}\n}""
    },
    ""response"": {
        ""paymentId"": null,
        ""paymentType"": ""account""
    }
}",
                new PaymentInfo
                {
                    PaymentId = null,
                    PaymentType = PaymentType.Account
                }
            ),
        };

        public static IEnumerable<TestCaseData> Checkout_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": \""string\"",\r\n  \""channelId\"": \""europa-test\"",\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""C\"",\r\n  \""deliveryCharge\"": \""245\"",\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": true,\r\n  \""paymentType\"": \""card\""\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""code"": ""booking_unknown_error"",
                ""message"": ""[Booking string] Failed to update booking details due to booking gateway unknown error [INSUFFDATA]: [Insuffient data has been supplied to complete this request.] (severity: E). Request data [{\""rqst\"":{\""pThwiCheckoutInW03\"":{\""refNo\"":\""string\"",\""tanSurname\"":\""BUTLER\"",\""booInitial\"":\""SHAWN\"",\""booTitle\"":\""MISS\"",\""booAddress1\"":\""Barnard's Inn\"",\""booAddress2\"":\""86 Fetter Lane\"",\""booAddress3\"":\""London\"",\""booAddress4\"":\""NY\"",\""booPostCode\"":\""EC4A 1EN\"",\""booCountryCode\"":\""UK\"",\""booPhone\"":\""07882571812\"",\""booEmailAddress\"":\""user@example.com\"",\""tksCode\"":\""C\"",\""flexiFlag\"":\""Y\"",\""mailChargePrice\"":0,\""voucherRecipient\"":\""Mr. Someone Else - Barnard's Inn, 86 Fetter Lane, London, EC4A 1EN, NY, United Kingdom\"",\""voucherMessage\"":\""Happy Birthday to you.\"",\""offCode\"":\""0\"",\""booChannelId\"":\""\"",\""booTxFxrRateFloat\"":0,\""promotion\"":{\""couponCode\"":\""\"",\""promotionCode\"":\""\""}}}}].""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "[Booking string] Failed to update booking details due to booking gateway unknown error [INSUFFDATA]: [Insuffient data has been supplied to complete this request.] (severity: E). Request data [{\"rqst\":{\"pThwiCheckoutInW03\":{\"refNo\":\"string\",\"tanSurname\":\"BUTLER\",\"booInitial\":\"SHAWN\",\"booTitle\":\"MISS\",\"booAddress1\":\"Barnard's Inn\",\"booAddress2\":\"86 Fetter Lane\",\"booAddress3\":\"London\",\"booAddress4\":\"NY\",\"booPostCode\":\"EC4A 1EN\",\"booCountryCode\":\"UK\",\"booPhone\":\"07882571812\",\"booEmailAddress\":\"user@example.com\",\"tksCode\":\"C\",\"flexiFlag\":\"Y\",\"mailChargePrice\":0,\"voucherRecipient\":\"Mr. Someone Else - Barnard's Inn, 86 Fetter Lane, London, EC4A 1EN, NY, United Kingdom\",\"voucherMessage\":\"Happy Birthday to you.\",\"offCode\":\"0\",\"booChannelId\":\"\",\"booTxFxrRateFloat\":0,\"promotion\":{\"couponCode\":\"\",\"promotionCode\":\"\"}}}}]."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": \""8526754\"",\r\n  \""channelId\"": \""europa-test\"",\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""C\"",\r\n  \""deliveryCharge\"": \""245\"",\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": true,\r\n  \""paymentType\"": \""card\""\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""code"": ""booking_expired"",
                ""message"": ""Cannot update booking with reference [8526754] that was expired.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Cannot update booking with reference [8526754] that was expired."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": \""8526788\"",\r\n  \""channelId\"": \""europa-test\"",\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""c\"",\r\n  \""deliveryCharge\"": 245,\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": true,\r\n  \""paymentType\"": \""card\""\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""field"": ""deliveryMethod"",
                ""message"": ""Please provide a valid delivery method. It should be set to either 'C' for COBO, 'E' for ETicket or 'M' for Postal delivery.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "deliveryMethod: Please provide a valid delivery method. It should be set to either 'C' for COBO, 'E' for ETicket or 'M' for Postal delivery."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": \""1\"",\r\n  \""channelId\"": \""europa-test\"",\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""C\"",\r\n  \""deliveryCharge\"": 245,\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": false,\r\n  \""paymentType\"": \""Account\""\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""code"": ""booking_not_found"",
                ""message"": ""Booking with reference [1] was not found.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Booking with reference [1] was not found."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": \""8527089\"",\r\n  \""channelId\"": \""eur44opa-test\"",\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""C\"",\r\n  \""deliveryCharge\"": 245,\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": false,\r\n  \""paymentType\"": \""Account\""\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Unable to find origin url for the provided channelId.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Unable to find origin url for the provided channelId."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\r\n  \""reference\"": null,\r\n  \""channelId\"": null,\r\n  \""shopper\"": {\r\n    \""email\"": \""user@example.com\"",\r\n    \""title\"": \""Miss\"",\r\n    \""firstName\"": \""Shawn\"",\r\n    \""lastName\"": \""Butler\"",\r\n    \""telephoneNumber\"": \""07882571812\"",\r\n    \""externalId\"": \""8263702\""\r\n  },\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""origin\"": \""https://example.com\"",\r\n  \""redirectUrl\"": \""https://example.com\"",\r\n  \""deliveryMethod\"": \""C\"",\r\n  \""deliveryCharge\"": 245,\r\n  \""recipientName\"": \""Mr. Someone Else\"",\r\n  \""giftVoucherMessage\"": \""Happy Birthday to you.\"",\r\n  \""deliveryAddress\"": {\r\n    \""line1\"": \""Barnard's Inn\"",\r\n    \""line2\"": \""86 Fetter Lane\"",\r\n    \""postalCode\"": \""EC4A 1EN\"",\r\n    \""city\"": \""London\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""countryName\"": \""United Kingdom\"",\r\n    \""stateOrProvince\"": \""NY\""\r\n  },\r\n  \""hasFlexiTickets\"": false,\r\n  \""paymentType\"": null\r\n}""
    },
    ""context"": {
        ""errors"": [
            {
                ""field"": ""reference"",
                ""message"": ""Please provide a booking reference number.""
            },
            {
                ""field"": ""channelId"",
                ""message"": ""Please provide a Channel ID.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "reference: Please provide a booking reference number.\r\nchannelId: Please provide a Channel ID."
            ),
        };

        #endregion

        #region ConfirmBooking

        public static IEnumerable<TestCaseData> ConfirmBooking_IfParametersAreSet_IfNotAgentBooking_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                "8527512",
                new ConfirmBookingParameters
                {
                    ChannelId = "resiaapi",
                    PaymentId = "your-payment-reference",
                    AgentPaymentReference = "your-reference"
                },
                "{\"channelId\":\"resiaapi\",\"paymentId\":\"your-payment-reference\",\"agentPaymentReference\":\"your-reference\"}"
            ),
        };

        public static IEnumerable<TestCaseData> ConfirmBooking_IfParametersAreSet_IfAgentBooking_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                "some_agent",
                "some_password",
                "8527512",
                new ConfirmBookingParameters
                {
                    ChannelId = "resiaapi",
                    PaymentId = "your-payment-reference",
                    AgentPaymentReference = "your-reference"
                },
                "{\"channelId\":\"resiaapi\",\"paymentId\":\"your-payment-reference\",\"agentPaymentReference\":\"your-reference\"}"
            ),
        };

        public static IEnumerable<TestCaseData> ConfirmBooking_IfApiResponseSuccessful_ReturnsBoolResult = new[]
        {
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\""channelId\"":\""resiaapi\"",\""paymentId\"":\""your-payment-reference\"",\""agentPaymentReference\"":\""your-reference\""}"",
        ""urlParams"": {
            ""referenceNo"": ""8527512""
        }
    },
    ""response"": {
        ""result"": ""success""
    }
}"
            ),
        };

        public static IEnumerable<TestCaseData> ConfirmBooking_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\""channelId\"":\""KyFiRKwzcp\"",\""paymentId\"":\""b88d1a1d-ea00-4142-8692-a46110d09dc5\"",\""agentPaymentReference\"":\""test-reference\""}"",
        ""urlParams"": {
            ""referenceNo"": ""8231754""
        }
    },
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Order for booking 8231754 and channel KyFiRKwzcp doesn't exists.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Order for booking 8231754 and channel KyFiRKwzcp doesn't exists."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\""channelId\"":\""europa-test\"",\""paymentId\"":\""90984ea1-add6-4320-8e2b-dcd362fe4eb9\"",\""agentPaymentReference\"":\""test-reference\""}"",
        ""urlParams"": {
            ""referenceNo"": ""8527491""
        }
    },
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Payment with id [90984ea1-add6-4320-8e2b-dcd362fe4eb9] is not authorised.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Payment with id [90984ea1-add6-4320-8e2b-dcd362fe4eb9] is not authorised."
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""body"": ""{\""channelId\"":\""resiaapi\"",\""paymentId\"":\""your-payment-reference\"",\""agentPaymentReference\"":\""your-reference\""}"",
        ""urlParams"": {
            ""referenceNo"": ""8527512""
        }
    },
    ""context"": {
        ""errors"": [
            {
                ""code"": ""booking_already_paid"",
                ""message"": ""Cannot confirm booking with reference [8527512] that is already paid.""
            }
        ]
    }
}",
                HttpStatusCode.BadRequest,
                "Cannot confirm booking with reference [8527512] that is already paid."
            ),

            // 403
            new TestCaseData(
                @"{
    ""request"": {},
    ""context"": {
        ""errors"": [
            {
                ""message"": ""Agent [resiaapi] is not authorized.""
            }
        ]
    }
}",
                HttpStatusCode.Forbidden,
                "Agent [resiaapi] is not authorized."
            ),
        };

        #endregion
    }
}
