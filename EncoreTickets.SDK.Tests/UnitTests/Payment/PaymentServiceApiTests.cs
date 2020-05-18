using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Authentication;
using EncoreTickets.SDK.Payment;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using Moq;
using NUnit.Framework;
using RestSharp;
using Address = EncoreTickets.SDK.Payment.Models.Address;

namespace EncoreTickets.SDK.Tests.UnitTests.Payment
{
    [TestFixture]
    internal class PaymentServiceApiTests : PaymentServiceApi
    {
        private const string TestValidChannelId = "channelId";
        private const string TestValidOrderExternalId = "externalId";
        private const string TestValidOrderId = "orderId";
        private const string CorrelationIdHeader = "X-Correlation-Id";

        private MockersForApiServiceWithAuthentication mockers;

        public override IAuthenticationService AuthenticationService => mockers.AuthenticationServiceMock.Object;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public PaymentServiceApiTests() : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiServiceWithAuthentication();
            ApiContextTestHelper.ResetContextToDefault(Context);
            Thread.CurrentThread.CurrentCulture = TestHelper.Culture;
        }

        #region Order

        #region GetOrder

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetOrder_IfChannelIdIsNotSet_ThrowsArgumentException(string channelId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetOrder(channelId, TestValidOrderExternalId);
            });
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetOrder_IfExternalIdIsNotSet_ThrowsArgumentException(string externalId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetOrder(TestValidChannelId, externalId);
            });
        }

        [TestCase(TestValidChannelId, TestValidOrderExternalId)]
        [TestCase("europa-prod", "1587")]
        public void GetOrder_CallsApiWithRightParameters(string channelId, string externalId)
        {
            mockers.SetupAnyExecution<ApiResponse<Order>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                GetOrder(channelId, externalId);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<Order>>(BaseUrl, $"v1/orders/{channelId}/{externalId}", Method.GET,
                expectedHeaders: new Dictionary<string, object> { [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.GetOrder_IfApiResponseSuccessful_ReturnsOrder))]
        public void GetOrder_IfApiResponseSuccessful_ReturnsOrder(
            string responseContent,
            Order expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<Order>>(responseContent);

            var actual = GetOrder(TestValidChannelId, TestValidOrderExternalId);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.GetOrder_IfApiResponseFailed_ThrowsApiException))]
        public void GetOrder_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<Order>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                GetOrder(TestValidChannelId, TestValidOrderExternalId);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region CreateOrder

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.CreateOrder_CallsApiWithRightParameters))]
        public void CreateOrder_CallsApiWithRightParameters(CreateOrderRequest order, string requestBody)
        {
            AutomaticAuthentication = true;
            mockers.SetupAnyExecution<ApiResponse<Order>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                CreateOrder(order);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyAuthenticateExecution(Times.Once());
            mockers.VerifyExecution<ApiResponse<Order>>(BaseUrl, "v1/orders", Method.POST,
                bodyInJson: requestBody,
                expectedHeaders: new Dictionary<string, object>{ [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.CreateOrder_IfApiResponseSuccessful_ReturnsCreatedOrder))]
        public void CreateOrder_IfApiResponseSuccessful_ReturnsCreatedOrder(
            string responseContent,
            Order expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<Order>>(responseContent);

            var actual = CreateOrder(It.IsAny<CreateOrderRequest>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.CreateOrder_IfApiResponseFailed_ThrowsApiException))]
        public void CreateOrder_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<Order>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = CreateOrder(It.IsAny<CreateOrderRequest>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #region UpdateOrder

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void UpdateOrder_IfOrderIdIsNotSet_ThrowsArgumentException(string orderId)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                var actual = UpdateOrder(orderId, It.IsAny<UpdateOrderRequest>());
            });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.UpdateOrder_CallsApiWithRightParameters))]
        public void UpdateOrder_CallsApiWithRightParameters(string orderId, UpdateOrderRequest order, string requestBody)
        {
            AutomaticAuthentication = true;
            mockers.SetupAnyExecution<ApiResponse<Order>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                UpdateOrder(orderId, order);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyAuthenticateExecution(Times.Once());
            mockers.VerifyExecution<ApiResponse<Order>>(BaseUrl, $"v1/orders/{orderId}", Method.PATCH,
                bodyInJson: requestBody,
                expectedHeaders: new Dictionary<string, object> { [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.UpdateOrder_IfApiResponseSuccessful_ReturnsUpdatedOrder))]
        public void UpdateOrder_IfApiResponseSuccessful_ReturnsUpdatedOrder(
            string responseContent,
            Order expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<Order>>(responseContent);

            var actual = UpdateOrder(TestValidOrderId, It.IsAny<UpdateOrderRequest>());

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.UpdateOrder_IfApiResponseFailed_ThrowsApiException))]
        public void UpdateOrder_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<Order>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = UpdateOrder(TestValidOrderId, It.IsAny<UpdateOrderRequest>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #endregion

        #region Country

        #region GetUsStates
        
        [Test]
        public void GetUsStates_CallsApiWithRightParameters()
        {
            mockers.SetupAnyExecution<ApiResponse<List<CountryTerritorialUnit>>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                GetUsStates();
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<List<CountryTerritorialUnit>>>(BaseUrl, "v1/countries/usa/states", Method.GET,
                expectedHeaders: new Dictionary<string, object> { [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.GetUsStates_IfApiResponseSuccessful_ReturnsUsStates))]
        public void GetUsStates_IfApiResponseSuccessful_ReturnsUsStates(
            string responseContent,
            List<CountryTerritorialUnit> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<List<CountryTerritorialUnit>>>(responseContent);

            var actual = GetUsStates();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        #endregion

        #region GetCanadaProvinces

        [Test]
        public void GetCanadaProvinces_CallsApiWithRightParameters()
        {
            mockers.SetupAnyExecution<ApiResponse<List<CountryTerritorialUnit>>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                GetCanadaProvinces();
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<List<CountryTerritorialUnit>>>(BaseUrl, 
                "v1/countries/canada/provinces", Method.GET,
                expectedHeaders: new Dictionary<string, object> { [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.GetCanadaProvinces_IfApiResponseSuccessful_ReturnsCanadaProvinces))]
        public void GetCanadaProvinces_IfApiResponseSuccessful_ReturnsCanadaProvinces(
            string responseContent,
            List<CountryTerritorialUnit> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<List<CountryTerritorialUnit>>>(responseContent);

            var actual = GetCanadaProvinces();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        #endregion

        #endregion

        #region Payment

        #region CreateNewPayment

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.CreateNewPayment_CallsApiWithRightParameters))]
        public void CreateNewPayment_CallsApiWithRightParameters(CreatePaymentRequest request, string requestBody)
        {
            AutomaticAuthentication = true;
            mockers.SetupAnyExecution<ApiResponse<SDK.Payment.Models.Payment>>();
            Context.Correlation = Guid.NewGuid().ToString();

            try
            {
                CreateNewPayment(request);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyAuthenticateExecution(Times.Once());
            mockers.VerifyExecution<ApiResponse<SDK.Payment.Models.Payment>>(BaseUrl, "v1/payments", Method.POST,
                bodyInJson: requestBody,
                expectedHeaders: new Dictionary<string, object> { [CorrelationIdHeader] = Context.Correlation });
        }

        [TestCaseSource(typeof(PaymentServiceApiTestsSource), nameof(PaymentServiceApiTestsSource.CreateNewPayment_IfApiResponseFailed_ThrowsApiException))]
        public void CreateNewPayment_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<SDK.Payment.Models.Payment>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = CreateNewPayment(It.IsAny<CreatePaymentRequest>());
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion

        #endregion
    }

    internal static class PaymentServiceApiTestsSource
    {
        #region Order

        public static IEnumerable<TestCaseData> GetOrder_IfApiResponseSuccessful_ReturnsOrder = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""urlParams"": {
      ""channelId"": ""localhost2"",
      ""externalId"": ""905909""
    }
  },
  ""response"": {
    ""id"": ""5b148b26-7e48-489e-8156-89534194f8a6"",
    ""createdAt"": ""2020-01-30T09:39:40+00:00"",
    ""channelId"": ""localhost2"",
    ""externalId"": ""905909"",
    ""redirectUrl"": ""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1"",
    ""origin"": ""http://localhost:8000"",
    ""payments"": [
      {
        ""id"": ""528f136e-54c3-4cbe-b5c3-585beda0c0a6"",
        ""createdAt"": ""2020-01-30T09:39:40+00:00"",
        ""amount"": {
          ""value"": 6200,
          ""currency"": ""GBP""
        },
        ""status"": ""new"",
        ""events"": [],
        ""refunds"": [],
        ""compensations"": []
      }
    ],
    ""shopper"": {
      ""email"": ""aburak@encore.co.uk"",
      ""firstName"": ""Aliaksei"",
      ""lastName"": ""Burak"",
      ""title"": ""Mr"",
      ""externalId"": ""ext-1""
    },
    ""billingAddress"": {
      ""line1"": ""Line1"",
      ""line2"": ""Line2"",
      ""postalCode"": ""AB1 2EF"",
      ""city"": ""Hometown"",
      ""countryCode"": ""GB"",
      ""legacyCountryCode"": ""UK""
    },
    ""items"": [
      {
        ""id"": ""605bd323-cf4a-4a50-a785-eb87960517e8"",
        ""name"": ""Book Of Mormon"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP""
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""3608""
      },
      {
        ""id"": ""c844050a-ec69-4842-9851-2bbfcf0d9610"",
        ""name"": ""Book Of Mormon"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP""
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""3608""
      }
    ],
    ""riskData"": {
      ""daysToEvent"": 2,
      ""deliveryMethod"": ""collection""
    }
  }
}",
                new Order
                {
                    Id = "5b148b26-7e48-489e-8156-89534194f8a6",
                    CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                    ChannelId = "localhost2",
                    ExternalId = "905909",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "528f136e-54c3-4cbe-b5c3-585beda0c0a6",
                            CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                            Amount = new Amount
                            {
                                Value = 6200,
                                Currency = "GBP"
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "aburak@encore.co.uk",
                        FirstName = "Aliaksei",
                        LastName = "Burak",
                        Title = "Mr",
                        ExternalId = "ext-1"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Line1",
                        Line2 = "Line2",
                        PostalCode = "AB1 2EF",
                        City = "Hometown",
                        CountryCode = "GB",
                        LegacyCountryCode = "UK"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "605bd323-cf4a-4a50-a785-eb87960517e8",
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                        new OrderItem
                        {
                            Id = "c844050a-ec69-4842-9851-2bbfcf0d9610",
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 2,
                        DeliveryMethod = "collection"
                    }
                }
            ),
            new TestCaseData(
                @"{
  ""request"": {
    ""urlParams"": {
      ""channelId"": ""europa-qa"",
      ""externalId"": ""808842""
    }
  },
  ""response"": {
    ""id"": ""2c542ed9-547a-46c1-9001-17666e2cfd9b"",
    ""createdAt"": ""2020-01-15T14:54:51+00:00"",
    ""channelId"": ""europa-qa"",
    ""externalId"": ""808842"",
    ""redirectUrl"": ""https://payment-service.qatixuk.io/redirect"",
    ""origin"": ""https://payment-service.qatixuk.io"",
    ""payments"": [
      {
        ""id"": ""6cd73a15-6d88-46ec-8876-653137257042"",
        ""createdAt"": ""2020-01-15T14:54:51+00:00"",
        ""amount"": {
          ""value"": 11800,
          ""currency"": ""GBP""
        },
        ""status"": ""partially_refunded"",
        ""events"": [
          {
            ""type"": ""capture"",
            ""createdAt"": ""2020-01-15T14:55:17+00:00"",
            ""pspReference"": ""881579100117023F"",
            ""pspCreatedAt"": ""2020-01-15T14:55:17+00:00"",
            ""status"": true
          },
          {
            ""type"": ""authorisation"",
            ""createdAt"": ""2020-01-15T14:55:15+00:00"",
            ""pspReference"": ""851579100114678C"",
            ""pspCreatedAt"": ""2020-01-15T14:55:15+00:00"",
            ""status"": true
          }
        ],
        ""refunds"": [
          {
            ""id"": ""15a9ea19-2c1b-4505-8a31-1b3a009eec91"",
            ""createdAt"": ""2020-01-15T15:13:17+00:00"",
            ""pspReference"": ""881579101197463E"",
            ""pspCreatedAt"": ""2020-01-15T15:13:17+00:00"",
            ""amount"": {
              ""value"": 100,
              ""currency"": ""GBP""
            },
            ""reason"": ""Test Booking"",
            ""status"": ""success""
          },
          {
            ""id"": ""e1bc3e9d-77ca-4cc3-ad29-97faa20f94ba"",
            ""createdAt"": ""2020-01-16T08:56:00+00:00"",
            ""pspReference"": ""881579164960934H"",
            ""pspCreatedAt"": ""2020-01-16T08:56:00+00:00"",
            ""amount"": {
              ""value"": 100,
              ""currency"": ""GBP""
            },
            ""reason"": ""Test Booking"",
            ""status"": ""success""
          }
        ],
        ""compensations"": [],
        ""paymentMethod"": {
          ""type"": ""card"",
          ""holderName"": ""RE"",
          ""scheme"": ""visa"",
          ""number"": ""444433******1111"",
          ""expiryDate"": ""10/2020""
        },
        ""merchantAccount"": ""EncoreTicketsCallCentre"",
        ""paymentServiceProvider"": ""AdyenTest""
      }
    ],
    ""shopper"": {
      ""lastName"": ""RE"",
      ""telephoneNumber"": ""re""
    },
    ""items"": [
      {
        ""id"": ""3fe70fd3-600d-4938-926d-053df976ad30"",
        ""name"": ""WICKED"",
        ""quantity"": 2,
        ""amount"": {
          ""value"": 5900,
          ""currency"": ""GBP""
        },
        ""externalId"": ""1587""
      }
    ],
    ""riskData"": {
      ""daysToEvent"": 0,
      ""deliveryMethod"": ""collection"",
      ""officeId"": ""1""
    }
  }
}",
                new Order
                {
                    Id = "2c542ed9-547a-46c1-9001-17666e2cfd9b",
                    CreatedAt = new DateTime(2020, 01, 15, 14, 54, 51),
                    ChannelId = "europa-qa",
                    ExternalId = "808842",
                    RedirectUrl = "https://payment-service.qatixuk.io/redirect",
                    Origin = "https://payment-service.qatixuk.io",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "6cd73a15-6d88-46ec-8876-653137257042",
                            CreatedAt = new DateTime(2020, 01, 15, 14, 54, 51),
                            Amount = new Amount
                            {
                                Value = 11800,
                                Currency = "GBP"
                            },
                            Status = "partially_refunded",
                            Events = new List<PaymentEvent>
                            {
                                new PaymentEvent
                                {
                                    Type = "capture",
                                    CreatedAt = new DateTime(2020, 01, 15, 14, 55, 17),
                                    PspReference = "881579100117023F",
                                    PspCreatedAt = new DateTime(2020, 01, 15, 14, 55, 17),
                                    Status = true
                                },
                                new PaymentEvent
                                {
                                    Type = "authorisation",
                                    CreatedAt = new DateTime(2020, 01, 15, 14, 55, 15),
                                    PspReference = "851579100114678C",
                                    PspCreatedAt = new DateTime(2020, 01, 15, 14, 55, 15),
                                    Status = true
                                },
                            },
                            Refunds = new List<Refund>
                            {
                                new Refund
                                {
                                    Id = "15a9ea19-2c1b-4505-8a31-1b3a009eec91",
                                    CreatedAt = new DateTime(2020, 01, 15, 15, 13, 17),
                                    PspReference = "881579101197463E",
                                    PspCreatedAt = new DateTime(2020, 01, 15, 15, 13, 17),
                                    Amount = new Amount
                                    {
                                        Value = 100,
                                        Currency = "GBP"
                                    },
                                    Reason = "Test Booking",
                                    Status = "success"
                                },
                                new Refund
                                {
                                    Id = "e1bc3e9d-77ca-4cc3-ad29-97faa20f94ba",
                                    CreatedAt = new DateTime(2020, 01, 16, 08, 56, 00),
                                    PspReference = "881579164960934H",
                                    PspCreatedAt = new DateTime(2020, 01, 16, 08, 56, 00),
                                    Amount = new Amount
                                    {
                                        Value = 100,
                                        Currency = "GBP"
                                    },
                                    Reason = "Test Booking",
                                    Status = "success"
                                },
                            },
                            Compensations = new List<Refund>(),
                            PaymentMethod = new PaymentMethod
                            {
                                Type = "card",
                                HolderName = "RE",
                                Scheme = "visa",
                                Number = "444433******1111",
                                ExpiryDate = new DateTime(2020, 10, 1)
                            },
                            PspMerchantAccount = "EncoreTicketsCallCentre",
                            PspName = "AdyenTest"
                        }
                    },
                    Shopper = new Shopper
                    {
                        LastName = "RE",
                        TelephoneNumber = "re"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "3fe70fd3-600d-4938-926d-053df976ad30",
                            Name = "WICKED",
                            Quantity = 2,
                            Amount = new Amount
                            {
                                Value = 5900,
                                Currency = "GBP"
                            },
                            ExternalId = "1587"
                        },
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 0,
                        DeliveryMethod = "collection",
                        OfficeId = 1
                    }
                }
            ),
            new TestCaseData(
                @"{
    ""request"": {
        ""urlParams"": {
            ""channelId"": "".net-sdk-integration-test"",
            ""externalId"": ""999999999""
        }
    },
    ""response"": {
        ""id"": ""d2941460-13e4-43f1-8a13-23a36bc1a714"",
        ""createdAt"": ""2020-02-03T09:00:29+00:00"",
        ""channelId"": "".net-sdk-integration-test"",
        ""externalId"": ""999999999"",
        ""redirectUrl"": ""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1"",
        ""origin"": ""http://localhost:8000"",
        ""payments"": [
            {
                ""id"": ""8495b8df-dcca-4b66-8ae6-96709b155b04"",
                ""createdAt"": ""2020-02-03T09:00:29+00:00"",
                ""amount"": {
                    ""value"": 11000,
                    ""currency"": ""GBP"",
                    ""exchangeRate"": 0.0
                },
                ""status"": ""new"",
                ""events"": [],
                ""refunds"": [],
                ""compensations"": []
            }
        ],
        ""shopper"": {
            ""email"": ""aburak@encore.co.uk"",
            ""firstName"": ""Tome1918607-70c1-492e-87bb-627122e99fef"",
            ""lastName"": ""Burak"",
            ""title"": ""Mr"",
            ""externalId"": ""ext-1""
        },
        ""billingAddress"": {
            ""line1"": ""Line1"",
            ""line2"": ""Addresse1918607-70c1-492e-87bb-627122e99fef"",
            ""postalCode"": ""AB1 2EF"",
            ""city"": ""Hometown"",
            ""countryCode"": ""GB""
        },
        ""items"": [
            {
                ""id"": ""843e8e93-705c-42d1-b6f8-121f953bd47f"",
                ""name"": ""Namee1918607-70c1-492e-87bb-627122e99fef"",
                ""description"": ""Online ticket sale Book Of Mormon"",
                ""quantity"": 1,
                ""amount"": {
                    ""value"": 5400,
                    ""currency"": ""GBP"",
                    ""exchangeRate"": 0.0
                },
                ""tax"": {
                    ""value"": 100,
                    ""currency"": ""GBP"",
                    ""exchangeRate"": 0.0
                },
                ""externalId"": ""3608""
            },
            {
                ""id"": ""cb3d9efc-653b-4938-9c4e-0d840600d865"",
                ""name"": ""Namee1918607-70c1-492e-87bb-627122e99fef"",
                ""description"": ""Online ticket sale Book Of Mormon"",
                ""quantity"": 1,
                ""amount"": {
                    ""value"": 5400,
                    ""currency"": ""GBP"",
                    ""exchangeRate"": 0.0
                },
                ""tax"": {
                    ""value"": 100,
                    ""currency"": ""GBP"",
                    ""exchangeRate"": 0.0
                },
                ""externalId"": ""3608""
            }
        ],
        ""riskData"": []
    }
}",
                new Order
                {
                    Id = "d2941460-13e4-43f1-8a13-23a36bc1a714",
                    CreatedAt = new DateTime(2020, 02, 03, 09, 00, 29),
                    ChannelId = ".net-sdk-integration-test",
                    ExternalId = "999999999",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "8495b8df-dcca-4b66-8ae6-96709b155b04",
                            CreatedAt = new DateTime(2020, 02, 03, 09, 00, 29),
                            Amount = new Amount
                            {
                                Value = 11000,
                                Currency = "GBP",
                                ExchangeRate = 0.0
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "aburak@encore.co.uk",
                        FirstName = "Tome1918607-70c1-492e-87bb-627122e99fef",
                        LastName = "Burak",
                        Title = "Mr",
                        ExternalId = "ext-1"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Line1",
                        Line2 = "Addresse1918607-70c1-492e-87bb-627122e99fef",
                        PostalCode = "AB1 2EF",
                        City = "Hometown",
                        CountryCode = "GB"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "843e8e93-705c-42d1-b6f8-121f953bd47f",
                            Name = "Namee1918607-70c1-492e-87bb-627122e99fef",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                        new OrderItem
                        {
                            Id = "cb3d9efc-653b-4938-9c4e-0d840600d865",
                            Name = "Namee1918607-70c1-492e-87bb-627122e99fef",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                    },
                    RiskData = null
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetOrder_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 404
            new TestCaseData(
                @"{
  ""request"": {
    ""urlParams"": {
      ""channelId"": ""localhost2"",
      ""externalId"": ""6690605""
    }
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""Cannot find Order. Please specify a valid orderId.""
      }
    ]
  }
}",
                HttpStatusCode.NotFound,
                "Cannot find Order. Please specify a valid orderId."
            ),
        };

        public static IEnumerable<TestCaseData> CreateOrder_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                new CreateOrderRequest
                {
                    ChannelId = "localhost2",
                    ExternalId = "905909",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Amount = new Amount
                    {
                        Value = 6200,
                        Currency = "GBP"
                    },
                    Shopper = new Shopper
                    {
                        Email = "aburak@encore.co.uk",
                        FirstName = "Aliaksei",
                        LastName = "Burak",
                        Title = "Mr",
                        ExternalId = "ext-1"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Line1",
                        Line2 = "Line2",
                        PostalCode = "AB1 2EF",
                        City = "Hometown",
                        CountryCode = "GB"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                        new OrderItem
                        {
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        }
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 2,
                        DeliveryMethod = "collection"
                    }
                },
                "{\"description\":null,\"channelId\":\"localhost2\",\"externalId\":\"905909\",\"redirectUrl\":\"https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1\",\"origin\":\"http://localhost:8000\",\"amount\":{\"value\":6200,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"billingAddress\":{\"line1\":\"Line1\",\"line2\":\"Line2\",\"postalCode\":\"AB1 2EF\",\"city\":\"Hometown\",\"countryCode\":\"GB\",\"legacyCountryCode\":null,\"stateOrProvince\":null},\"shopper\":{\"email\":\"aburak@encore.co.uk\",\"firstName\":\"Aliaksei\",\"lastName\":\"Burak\",\"telephoneNumber\":null,\"title\":\"Mr\",\"externalId\":\"ext-1\",\"locale\":null},\"items\":[{\"id\":null,\"name\":\"Book Of Mormon\",\"description\":\"Online ticket sale Book Of Mormon\",\"quantity\":1,\"amount\":{\"value\":5400,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"tax\":{\"value\":100,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"externalId\":\"3608\"},{\"id\":null,\"name\":\"Book Of Mormon\",\"description\":\"Online ticket sale Book Of Mormon\",\"quantity\":1,\"amount\":{\"value\":5400,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"tax\":{\"value\":100,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"externalId\":\"3608\"}],\"riskData\":{\"deliveryMethod\":\"collection\",\"officeId\":null,\"daysToEvent\":2}}"
            ),
            new TestCaseData(
                new CreateOrderRequest
                {
                    ChannelId = "europa-qa",
                    ExternalId = "889454",
                    RedirectUrl = "https://payment-service.qatixuk.io/redirect",
                    Origin = "https://payment-service.qatixuk.io",
                    Amount = new Amount
                    {
                        Value = 8100,
                        Currency = "GBP"
                    },
                    Shopper = new Shopper
                    {
                        Email = "test@test.com",
                        TelephoneNumber = "02072578183",
                        Title = "MS",
                        FirstName = "INNA",
                        LastName = "IVANOVA",
                        ExternalId = null
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "115 Shaftesbury Avenue",
                        Line2 = null,
                        PostalCode = "WC2H 8AF",
                        City = "Cambridge Circus",
                        CountryCode = "UK",
                        LegacyCountryCode = null,
                        StateOrProvince = "London"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Name = "WICKED",
                            Description = null,
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 8100,
                                Currency = "GBP"
                            },
                            Tax = null,
                            ExternalId = "1587"
                        }
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 0,
                        DeliveryMethod = "collection",
                        OfficeId = 1
                    }
                },
                "{\"description\":null,\"channelId\":\"europa-qa\",\"externalId\":\"889454\",\"redirectUrl\":\"https://payment-service.qatixuk.io/redirect\",\"origin\":\"https://payment-service.qatixuk.io\",\"amount\":{\"value\":8100,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"billingAddress\":{\"line1\":\"115 Shaftesbury Avenue\",\"line2\":null,\"postalCode\":\"WC2H 8AF\",\"city\":\"Cambridge Circus\",\"countryCode\":\"UK\",\"legacyCountryCode\":null,\"stateOrProvince\":\"London\"},\"shopper\":{\"email\":\"test@test.com\",\"firstName\":\"INNA\",\"lastName\":\"IVANOVA\",\"telephoneNumber\":\"02072578183\",\"title\":\"MS\",\"externalId\":null,\"locale\":null},\"items\":[{\"id\":null,\"name\":\"WICKED\",\"description\":null,\"quantity\":1,\"amount\":{\"value\":8100,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"tax\":null,\"externalId\":\"1587\"}],\"riskData\":{\"deliveryMethod\":\"collection\",\"officeId\":1,\"daysToEvent\":0}}"
            ),
        };

        public static IEnumerable<TestCaseData> CreateOrder_IfApiResponseSuccessful_ReturnsCreatedOrder = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\n    \""channelId\"": \""localhost2\"",\n    \""externalId\"": \""905909\"",\n    \""redirectUrl\"": \""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1\"",\n    \""origin\"": \""http://localhost:8000\"",\n    \""amount\"": {\n        \""value\"": 6200,\n        \""currency\"": \""GBP\""\n    },\n    \""shopper\"": {\n        \""email\"": \""aburak@encore.co.uk\"",\n        \""title\"": \""Mr\"",\n        \""firstName\"": \""Aliaksei\"",\n        \""lastName\"": \""Burak\"",\n        \""externalId\"": \""ext-1\""\n    },\n    \""billingAddress\"": {\n        \""line1\"": \""Line1\"",\n        \""line2\"": \""Line2\"",\n        \""postalCode\"": \""AB1 2EF\"",\n        \""city\"": \""Hometown\"",\n        \""countryCode\"": \""UK\""\n    },\n    \""items\"": [\n        {\n            \""name\"": \""Book Of Mormon\"",\n            \""description\"": \""Online ticket sale Book Of Mormon\"",\n            \""quantity\"": 1,\n            \""externalId\"": \""3608\"",\n            \""amount\"": {\n                \""value\"": 5400,\n                \""currency\"": \""GBP\""\n            },\n            \""tax\"": {\n                \""value\"": 100,\n                \""currency\"": \""GBP\""\n            }\n        },\n        {\n            \""name\"": \""Book Of Mormon\"",\n            \""description\"": \""Online ticket sale Book Of Mormon\"",\n            \""quantity\"": 1,\n            \""externalId\"": \""3608\"",\n            \""amount\"": {\n                \""value\"": 5400,\n                \""currency\"": \""GBP\""\n            },\n            \""tax\"": {\n                \""value\"": 100,\n                \""currency\"": \""GBP\""\n            }\n        }\n    ],\n    \""riskData\"": {\n        \""daysToEvent\"": \""2\"",\n        \""deliveryMethod\"": \""collection\""\n    }\n}""
  },
  ""response"": {
    ""id"": ""5b148b26-7e48-489e-8156-89534194f8a6"",
    ""createdAt"": ""2020-01-30T09:39:40+00:00"",
    ""channelId"": ""localhost2"",
    ""externalId"": ""905909"",
    ""redirectUrl"": ""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1"",
    ""origin"": ""http://localhost:8000"",
    ""payments"": [
      {
        ""id"": ""528f136e-54c3-4cbe-b5c3-585beda0c0a6"",
        ""createdAt"": ""2020-01-30T09:39:40+00:00"",
        ""amount"": {
          ""value"": 6200,
          ""currency"": ""GBP""
        },
        ""status"": ""new"",
        ""events"": [],
        ""refunds"": [],
        ""compensations"": []
      }
    ],
    ""shopper"": {
      ""email"": ""aburak@encore.co.uk"",
      ""firstName"": ""Aliaksei"",
      ""lastName"": ""Burak"",
      ""title"": ""Mr"",
      ""externalId"": ""ext-1""
    },
    ""billingAddress"": {
      ""line1"": ""Line1"",
      ""line2"": ""Line2"",
      ""postalCode"": ""AB1 2EF"",
      ""city"": ""Hometown"",
      ""countryCode"": ""GB"",
      ""legacyCountryCode"": ""UK""
    },
    ""items"": [
      {
        ""id"": ""c844050a-ec69-4842-9851-2bbfcf0d9610"",
        ""name"": ""Book Of Mormon"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP""
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""3608""
      },
      {
        ""id"": ""605bd323-cf4a-4a50-a785-eb87960517e8"",
        ""name"": ""Book Of Mormon"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP""
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""3608""
      }
    ],
    ""riskData"": {
      ""daysToEvent"": 2,
      ""deliveryMethod"": ""collection""
    }
  }
}",
                new Order
                {
                    Id = "5b148b26-7e48-489e-8156-89534194f8a6",
                    CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                    ChannelId = "localhost2",
                    ExternalId = "905909",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "528f136e-54c3-4cbe-b5c3-585beda0c0a6",
                            CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                            Amount = new Amount
                            {
                                Value = 6200,
                                Currency = "GBP"
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "aburak@encore.co.uk",
                        FirstName = "Aliaksei",
                        LastName = "Burak",
                        Title = "Mr",
                        ExternalId = "ext-1"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Line1",
                        Line2 = "Line2",
                        PostalCode = "AB1 2EF",
                        City = "Hometown",
                        CountryCode = "GB",
                        LegacyCountryCode = "UK"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "c844050a-ec69-4842-9851-2bbfcf0d9610",
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                        new OrderItem
                        {
                            Id = "605bd323-cf4a-4a50-a785-eb87960517e8",
                            Name = "Book Of Mormon",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        }
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 2,
                        DeliveryMethod = "collection"
                    }
                }
            ),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\""channelId\"":\""europa-qa\"",\""externalId\"":\""889454\"",\""redirectUrl\"":\""https://payment-service.qatixuk.io/redirect\"",\""origin\"":\""https://payment-service.qatixuk.io\"",\""amount\"":{\""value\"":8100,\""currency\"":\""GBP\""},\""billingAddress\"":{\""line1\"":\""115 Shaftesbury Avenue\"",\""line2\"":null,\""postalCode\"":\""WC2H 8AF\"",\""city\"":\""Cambridge Circus\"",\""countryCode\"":\""UK\"",\""legacyCountryCode\"":null,\""stateOrProvince\"":\""London\""},\""shopper\"":{\""email\"":\""test@test.com\"",\""telephoneNumber\"":\""02072578183\"",\""title\"":\""MS\"",\""firstName\"":\""INNA\"",\""lastName\"":\""IVANOVA\"",\""externalId\"":null},\""items\"":[{\""name\"":\""WICKED\"",\""description\"":null,\""quantity\"":1,\""externalId\"":\""1587\"",\""amount\"":{\""value\"":8100,\""currency\"":\""GBP\""},\""tax\"":null}],\""riskData\"":{\""deliveryMethod\"":\""collection\"",\""officeId\"":1,\""daysToEvent\"":0}}""
  },
  ""response"": {
    ""id"": ""cffb7c33-5ee6-410d-b975-48fc0546aab0"",
    ""createdAt"": ""2020-01-31T07:32:38+00:00"",
    ""channelId"": ""europa-qa"",
    ""externalId"": ""889454"",
    ""redirectUrl"": ""https://payment-service.qatixuk.io/redirect"",
    ""origin"": ""https://payment-service.qatixuk.io"",
    ""payments"": [
      {
        ""id"": ""67d2743e-52e7-4767-af69-a4988f91e4e2"",
        ""createdAt"": ""2020-01-31T07:32:38+00:00"",
        ""amount"": {
          ""value"": 8100,
          ""currency"": ""GBP""
        },
        ""status"": ""new"",
        ""events"": [],
        ""refunds"": [],
        ""compensations"": []
      }
    ],
    ""shopper"": {
      ""email"": ""test@test.com"",
      ""firstName"": ""INNA"",
      ""lastName"": ""IVANOVA"",
      ""telephoneNumber"": ""02072578183"",
      ""title"": ""MS""
    },
    ""billingAddress"": {
      ""line1"": ""115 Shaftesbury Avenue"",
      ""postalCode"": ""WC2H 8AF"",
      ""city"": ""Cambridge Circus"",
      ""countryCode"": ""GB"",
      ""legacyCountryCode"": ""UK"",
      ""stateOrProvince"": ""London""
    },
    ""items"": [
      {
        ""id"": ""12b78a4d-4982-4da9-952f-8a9a6290194d"",
        ""name"": ""WICKED"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 8100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""1587""
      }
    ],
    ""riskData"": {
      ""daysToEvent"": 0,
      ""deliveryMethod"": ""collection"",
      ""officeId"": ""1""
    }
  }
}",
                new Order
                {
                    Id = "cffb7c33-5ee6-410d-b975-48fc0546aab0",
                    CreatedAt = new DateTime(2020, 01, 31, 07, 32, 38),
                    ChannelId = "europa-qa",
                    ExternalId = "889454",
                    RedirectUrl = "https://payment-service.qatixuk.io/redirect",
                    Origin = "https://payment-service.qatixuk.io",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "67d2743e-52e7-4767-af69-a4988f91e4e2",
                            CreatedAt = new DateTime(2020, 01, 31, 07, 32, 38),
                            Amount = new Amount
                            {
                                Value = 8100,
                                Currency = "GBP"
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "test@test.com",
                        FirstName = "INNA",
                        LastName = "IVANOVA",
                        TelephoneNumber = "02072578183",
                        Title = "MS"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "115 Shaftesbury Avenue",
                        PostalCode = "WC2H 8AF",
                        City = "Cambridge Circus",
                        CountryCode = "GB",
                        LegacyCountryCode = "UK",
                        StateOrProvince = "London"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "12b78a4d-4982-4da9-952f-8a9a6290194d",
                            Name = "WICKED",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 8100,
                                Currency = "GBP"
                            },
                            ExternalId = "1587"
                        },
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 0,
                        DeliveryMethod = "collection",
                        OfficeId = 1
                    }
                }
            ),
        };

        public static IEnumerable<TestCaseData> CreateOrder_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\r\n  \""description\"": null,\r\n  \""channelId\"": \""localhost2\"",\r\n  \""externalId\"": \""905909\"",\r\n  \""redirectUrl\"": \""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1\"",\r\n  \""origin\"": \""http://localhost:8000\"",\r\n  \""amount\"": {\r\n    \""value\"": 6200,\r\n    \""currency\"": \""GBP\"",\r\n    \""exchangeRate\"": 0\r\n  },\r\n  \""amountOriginal\"": null,\r\n  \""billingAddress\"": {\r\n    \""line1\"": \""Line1\"",\r\n    \""line2\"": \""Line2\"",\r\n    \""postalCode\"": \""AB1 2EF\"",\r\n    \""city\"": \""Hometown\"",\r\n    \""countryCode\"": \""GB\"",\r\n    \""legacyCountryCode\"": null,\r\n    \""stateOrProvince\"": null\r\n  },\r\n  \""shopper\"": {\r\n    \""email\"": \""aburak@encore.co.uk\"",\r\n    \""firstName\"": \""Aliaksei\"",\r\n    \""lastName\"": \""Burak\"",\r\n    \""telephoneNumber\"": null,\r\n    \""title\"": \""Mr\"",\r\n    \""externalId\"": \""ext-1\"",\r\n    \""locale\"": null\r\n  },\r\n  \""items\"": [\r\n    {\r\n      \""id\"": null,\r\n      \""name\"": \""Book Of Mormon\"",\r\n      \""description\"": \""Online ticket sale Book Of Mormon\"",\r\n      \""quantity\"": 1,\r\n      \""amount\"": {\r\n        \""value\"": 5400,\r\n        \""currency\"": \""GBP\"",\r\n        \""exchangeRate\"": 0\r\n      },\r\n      \""amountOriginal\"": null,\r\n      \""tax\"": {\r\n        \""value\"": 100,\r\n        \""currency\"": \""GBP\"",\r\n        \""exchangeRate\"": 0\r\n      },\r\n      \""externalId\"": \""3608\""\r\n    },\r\n    {\r\n      \""id\"": null,\r\n      \""name\"": \""Book Of Mormon\"",\r\n      \""description\"": \""Online ticket sale Book Of Mormon\"",\r\n      \""quantity\"": 1,\r\n      \""amount\"": {\r\n        \""value\"": 5400,\r\n        \""currency\"": \""GBP\"",\r\n        \""exchangeRate\"": 0\r\n      },\r\n      \""amountOriginal\"": null,\r\n      \""tax\"": {\r\n        \""value\"": 100,\r\n        \""currency\"": \""GBP\"",\r\n        \""exchangeRate\"": 0\r\n      },\r\n      \""externalId\"": \""3608\""\r\n    }\r\n  ],\r\n  \""riskData\"": {\r\n    \""deliveryMethod\"": \""collection\"",\r\n    \""officeId\"": null,\r\n    \""daysToEvent\"": 2\r\n  }\r\n}""
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""Order already exist for given channelId (localhost2) and externalId (905909)""
      }
    ]
  }
}",
                HttpStatusCode.BadRequest,
                "Order already exist for given channelId (localhost2) and externalId (905909)"
            ),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\""description\"":\""test description\"",\""channelId\"":\"".net-sdk-integration-test\"",\""externalId\"":\""987654321\"",\""redirectUrl\"":\""https://payment-service.qatixuk.io/redirect\"",\""origin\"":\""https://payment-service.qatixuk.io\"",\""amount\"":{\""value\"":8100,\""currency\"":\""USD\"",\""exchangeRate\"":1.2},\""amountOriginal\"":{\""value\"":100000,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""billingAddress\"":{\""line1\"":\""115 Shaftesbury Avenue\"",\""line2\"":null,\""postalCode\"":\""WC2H 8AF\"",\""city\"":\""Cambridge Circus\"",\""countryCode\"":\""UK\"",\""legacyCountryCode\"":null,\""stateOrProvince\"":\""London\""},\""shopper\"":{\""email\"":\""test@test.com\"",\""firstName\"":\""INNA\"",\""lastName\"":\""IVANOVA\"",\""telephoneNumber\"":\""02072578183\"",\""title\"":\""MS\"",\""externalId\"":null,\""locale\"":null},\""items\"":[{\""id\"":null,\""name\"":\""WICKED\"",\""description\"":null,\""quantity\"":1,\""amount\"":{\""value\"":8100,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""amountOriginal\"":null,\""tax\"":null,\""externalId\"":\""1587\""}],\""riskData\"":{\""deliveryMethod\"":\""collection\"",\""officeId\"":1,\""daysToEvent\"":0}}""
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""Unable to create the order. Please check that all your currencies are identical or create a separate order""
      }
    ]
  }
}",
                HttpStatusCode.BadRequest,
                "Unable to create the order. Please check that all your currencies are identical or create a separate order"
            ),

            // 401
            new TestCaseData(
                "{\"code\":401,\"message\":\"Invalid JWT Token\"}",
                HttpStatusCode.Unauthorized,
                "Invalid JWT Token"
            ),
        };

        public static IEnumerable<TestCaseData> UpdateOrder_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                "5b148b26-7e48-489e-8156-89534194f8a6",
                new UpdateOrderRequest
                {
                    Shopper = new Shopper
                    {
                        Email = "newshopper@email.tld",
                        FirstName = "newJohn",
                        LastName = "newDoe",
                        Title = "newMr",
                        ExternalId = "newext-1",
                        TelephoneNumber = "new+441234567890",
                        Locale = "fr_FR"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "newHouse 1",
                        Line2 = "new123 street",
                        PostalCode = "newAB1 2EF",
                        City = "newHometown",
                        CountryCode = "FR"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Name = "newLion King",
                            Description = "newOnline ticket sale The lion king",
                            Quantity = 2,
                            Amount = new Amount
                            {
                                Value = 3000,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "123"
                        }
                    }
                },
                "{\"billingAddress\":{\"line1\":\"newHouse 1\",\"line2\":\"new123 street\",\"postalCode\":\"newAB1 2EF\",\"city\":\"newHometown\",\"countryCode\":\"FR\",\"legacyCountryCode\":null,\"stateOrProvince\":null},\"shopper\":{\"email\":\"newshopper@email.tld\",\"firstName\":\"newJohn\",\"lastName\":\"newDoe\",\"telephoneNumber\":\"new+441234567890\",\"title\":\"newMr\",\"externalId\":\"newext-1\",\"locale\":\"fr_FR\"},\"items\":[{\"id\":null,\"name\":\"newLion King\",\"description\":\"newOnline ticket sale The lion king\",\"quantity\":2,\"amount\":{\"value\":3000,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null,\"tax\":{\"value\":100,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"externalId\":\"123\"}],\"riskData\":null}"
            ),
        };

        public static IEnumerable<TestCaseData> UpdateOrder_IfApiResponseSuccessful_ReturnsUpdatedOrder = new[]
        {
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\n    \""shopper\"": {\n        \""email\"": \""newshopper@email.tld\"",\n        \""firstName\"": \""newJohn\"",\n        \""lastName\"": \""newDoe\"",\n        \""title\"": \""newMr\"",\n        \""externalId\"": \""newext-1\"",\n        \""telephoneNumber\"": \""new+441234567890\"",\n        \""locale\"": \""fr_FR\""\n    },\n    \""billingAddress\"": {\n        \""line1\"": \""newHouse 1\"",\n        \""line2\"": \""new123 street\"",\n        \""postalCode\"": \""newAB1 2EF\"",\n        \""city\"": \""newHometown\"",\n        \""countryCode\"": \""FR\""\n    },\n    \""items\"": [\n        {\n            \""name\"": \""newLion King\"",\n            \""description\"": \""newOnline ticket sale The lion king\"",\n            \""quantity\"": 2,\n            \""amount\"": {\n                \""value\"": 3000,\n                \""currency\"": \""GBP\""\n            },\n            \""tax\"": {\n                \""value\"": 100,\n                \""currency\"": \""GBP\""\n            },\n            \""externalId\"": \""123\""\n        }   \n    ]\n}"",
    ""urlParams"": {
      ""id"": ""5b148b26-7e48-489e-8156-89534194f8a6""
    }
  },
  ""response"": {
    ""id"": ""5b148b26-7e48-489e-8156-89534194f8a6"",
    ""createdAt"": ""2020-01-30T09:39:40+00:00"",
    ""channelId"": ""localhost2"",
    ""externalId"": ""905909"",
    ""redirectUrl"": ""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1"",
    ""origin"": ""http://localhost:8000"",
    ""payments"": [
      {
        ""id"": ""528f136e-54c3-4cbe-b5c3-585beda0c0a6"",
        ""createdAt"": ""2020-01-30T09:39:40+00:00"",
        ""amount"": {
          ""value"": 6200,
          ""currency"": ""GBP""
        },
        ""status"": ""new"",
        ""events"": [],
        ""refunds"": [],
        ""compensations"": []
      }
    ],
    ""shopper"": {
      ""email"": ""newshopper@email.tld"",
      ""firstName"": ""newJohn"",
      ""lastName"": ""newDoe"",
      ""telephoneNumber"": ""new+441234567890"",
      ""title"": ""newMr"",
      ""externalId"": ""newext-1"",
      ""locale"": ""fr_FR""
    },
    ""billingAddress"": {
      ""line1"": ""newHouse 1"",
      ""line2"": ""new123 street"",
      ""postalCode"": ""newAB1 2EF"",
      ""city"": ""newHometown"",
      ""countryCode"": ""FR""
    },
    ""items"": [
      {
        ""id"": ""bcd00dca-674b-4506-9604-8c6d8bd8421f"",
        ""name"": ""newLion King"",
        ""description"": ""newOnline ticket sale The lion king"",
        ""quantity"": 2,
        ""amount"": {
          ""value"": 3000,
          ""currency"": ""GBP""
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP""
        },
        ""externalId"": ""123""
      }
    ],
    ""riskData"": {
      ""daysToEvent"": 2,
      ""deliveryMethod"": ""collection""
    }
  }
}",
                new Order
                {
                    Id = "5b148b26-7e48-489e-8156-89534194f8a6",
                    CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                    ChannelId = "localhost2",
                    ExternalId = "905909",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "528f136e-54c3-4cbe-b5c3-585beda0c0a6",
                            CreatedAt = new DateTime(2020, 01, 30, 09, 39, 40),
                            Amount = new Amount
                            {
                                Value = 6200,
                                Currency = "GBP"
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "newshopper@email.tld",
                        FirstName = "newJohn",
                        LastName = "newDoe",
                        Title = "newMr",
                        ExternalId = "newext-1",
                        TelephoneNumber = "new+441234567890",
                        Locale = "fr_FR"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "newHouse 1",
                        Line2 = "new123 street",
                        PostalCode = "newAB1 2EF",
                        City = "newHometown",
                        CountryCode = "FR"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "bcd00dca-674b-4506-9604-8c6d8bd8421f",
                            Name = "newLion King",
                            Description = "newOnline ticket sale The lion king",
                            Quantity = 2,
                            Amount = new Amount
                            {
                                Value = 3000,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "123"
                        }
                    },
                    RiskData = new RiskData
                    {
                        DaysToEvent = 2,
                        DeliveryMethod = "collection"
                    }
                }
            ),
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\""billingAddress\"":{\""line1\"":\""Line1\"",\""line2\"":\""Address7ad4a9f9-0654-43f1-b51b-1f36a24bd62a\"",\""postalCode\"":\""AB1 2EF\"",\""city\"":\""Hometown\"",\""countryCode\"":\""GB\"",\""legacyCountryCode\"":null,\""stateOrProvince\"":null},\""shopper\"":{\""email\"":\""aburak@encore.co.uk\"",\""firstName\"":\""Tom7ad4a9f9-0654-43f1-b51b-1f36a24bd62a\"",\""lastName\"":\""Burak\"",\""telephoneNumber\"":null,\""title\"":\""Mr\"",\""externalId\"":\""ext-1\"",\""locale\"":null},\""items\"":[{\""id\"":\""422ac918-fc09-466c-b2fe-051537e1d967\"",\""name\"":\""Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a\"",\""description\"":\""Online ticket sale Book Of Mormon\"",\""quantity\"":1,\""amount\"":{\""value\"":5400,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""amountOriginal\"":null,\""tax\"":{\""value\"":100,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""externalId\"":\""3608\""},{\""id\"":\""5a230459-457e-4848-ad99-4561e62380a0\"",\""name\"":\""Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a\"",\""description\"":\""Online ticket sale Book Of Mormon\"",\""quantity\"":1,\""amount\"":{\""value\"":5400,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""amountOriginal\"":null,\""tax\"":{\""value\"":100,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""externalId\"":\""3608\""}],\""riskData\"":{\""deliveryMethod\"":null,\""officeId\"":null,\""daysToEvent\"":null}}"",
    ""urlParams"": {
      ""id"": ""d2941460-13e4-43f1-8a13-23a36bc1a714""
    }
  },
  ""response"": {
    ""id"": ""d2941460-13e4-43f1-8a13-23a36bc1a714"",
    ""createdAt"": ""2020-02-03T09:00:29+00:00"",
    ""channelId"": "".net-sdk-integration-test"",
    ""externalId"": ""999999999"",
    ""redirectUrl"": ""https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1"",
    ""origin"": ""http://localhost:8000"",
    ""payments"": [
      {
        ""id"": ""8495b8df-dcca-4b66-8ae6-96709b155b04"",
        ""createdAt"": ""2020-02-03T09:00:29+00:00"",
        ""amount"": {
          ""value"": 11000,
          ""currency"": ""GBP"",
          ""exchangeRate"": 0
        },
        ""status"": ""new"",
        ""events"": [],
        ""refunds"": [],
        ""compensations"": []
      }
    ],
    ""shopper"": {
      ""email"": ""aburak@encore.co.uk"",
      ""firstName"": ""Tom7ad4a9f9-0654-43f1-b51b-1f36a24bd62a"",
      ""lastName"": ""Burak"",
      ""title"": ""Mr"",
      ""externalId"": ""ext-1""
    },
    ""billingAddress"": {
      ""line1"": ""Line1"",
      ""line2"": ""Address7ad4a9f9-0654-43f1-b51b-1f36a24bd62a"",
      ""postalCode"": ""AB1 2EF"",
      ""city"": ""Hometown"",
      ""countryCode"": ""GB""
    },
    ""items"": [
      {
        ""id"": ""7b656d9a-cfb9-4706-b448-06a4213dc215"",
        ""name"": ""Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP"",
          ""exchangeRate"": 0
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP"",
          ""exchangeRate"": 0
        },
        ""externalId"": ""3608""
      },
      {
        ""id"": ""f6c3faf3-c12a-4082-a396-5040ef8e9e93"",
        ""name"": ""Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a"",
        ""description"": ""Online ticket sale Book Of Mormon"",
        ""quantity"": 1,
        ""amount"": {
          ""value"": 5400,
          ""currency"": ""GBP"",
          ""exchangeRate"": 0
        },
        ""tax"": {
          ""value"": 100,
          ""currency"": ""GBP"",
          ""exchangeRate"": 0
        },
        ""externalId"": ""3608""
      }
    ],
    ""riskData"": []
  }
}",
                new Order
                {
                    Id = "d2941460-13e4-43f1-8a13-23a36bc1a714",
                    CreatedAt = new DateTime(2020, 02, 03, 09, 00, 29),
                    ChannelId = ".net-sdk-integration-test",
                    ExternalId = "999999999",
                    RedirectUrl =
                        "https://londontheatredd.wl.front-default.bb-qa6.qa.encoretix.co.uk/checkout#/payment-details?reference=6836136&checksum=A8B6ED89A1",
                    Origin = "http://localhost:8000",
                    Payments = new List<SDK.Payment.Models.Payment>
                    {
                        new SDK.Payment.Models.Payment
                        {
                            Id = "8495b8df-dcca-4b66-8ae6-96709b155b04",
                            CreatedAt = new DateTime(2020, 02, 03, 09, 00, 29),
                            Amount = new Amount
                            {
                                Value = 11000,
                                Currency = "GBP",
                                ExchangeRate = 0.0
                            },
                            Status = "new",
                            Events = new List<PaymentEvent>(),
                            Refunds = new List<Refund>(),
                            Compensations = new List<Refund>()
                        }
                    },
                    Shopper = new Shopper
                    {
                        Email = "aburak@encore.co.uk",
                        FirstName = "Tom7ad4a9f9-0654-43f1-b51b-1f36a24bd62a",
                        LastName = "Burak",
                        Title = "Mr",
                        ExternalId = "ext-1"
                    },
                    BillingAddress = new Address
                    {
                        Line1 = "Line1",
                        Line2 = "Address7ad4a9f9-0654-43f1-b51b-1f36a24bd62a",
                        PostalCode = "AB1 2EF",
                        City = "Hometown",
                        CountryCode = "GB"
                    },
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Id = "7b656d9a-cfb9-4706-b448-06a4213dc215",
                            Name = "Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                        new OrderItem
                        {
                            Id = "f6c3faf3-c12a-4082-a396-5040ef8e9e93",
                            Name = "Name7ad4a9f9-0654-43f1-b51b-1f36a24bd62a",
                            Description = "Online ticket sale Book Of Mormon",
                            Quantity = 1,
                            Amount = new Amount
                            {
                                Value = 5400,
                                Currency = "GBP"
                            },
                            Tax = new Amount
                            {
                                Value = 100,
                                Currency = "GBP"
                            },
                            ExternalId = "3608"
                        },
                    },
                    RiskData = null
                }
            ),
        };

        public static IEnumerable<TestCaseData> UpdateOrder_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\""billingAddress\"":{\""line1\"":\""Line1\"",\""line2\"":\""Address9774e00d-2265-4f5c-9249-4ee9667dbbd2\"",\""postalCode\"":\""AB1 2EF\"",\""city\"":\""Hometown\"",\""countryCode\"":\""GB\"",\""legacyCountryCode\"":null,\""stateOrProvince\"":null},\""shopper\"":{\""email\"":\""aburak@encore.co.uk\"",\""firstName\"":\""Tom9774e00d-2265-4f5c-9249-4ee9667dbbd2\"",\""lastName\"":\""Burak\"",\""telephoneNumber\"":null,\""title\"":\""Mr\"",\""externalId\"":\""ext-1\"",\""locale\"":null},\""items\"":[{\""id\"":\""422ac918-fc09-466c-b2fe-051537e1d967\"",\""name\"":\""Name9774e00d-2265-4f5c-9249-4ee9667dbbd2\"",\""description\"":\""Online ticket sale Book Of Mormon\"",\""quantity\"":1,\""amount\"":{\""value\"":5400,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""amountOriginal\"":null,\""tax\"":{\""value\"":100,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""externalId\"":\""3608\""},{\""id\"":\""5a230459-457e-4848-ad99-4561e62380a0\"",\""name\"":\""Name9774e00d-2265-4f5c-9249-4ee9667dbbd2\"",\""description\"":\""Online ticket sale Book Of Mormon\"",\""quantity\"":1,\""amount\"":{\""value\"":5400,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""amountOriginal\"":null,\""tax\"":{\""value\"":100,\""currency\"":\""GBP\"",\""exchangeRate\"":0.0},\""externalId\"":\""3608\""}],\""riskData\"":{\""deliveryMethod\"":\""DeliveryMethod9774e00d-2265-4f5c-9249-4ee9667dbbd2\"",\""officeId\"":null,\""daysToEvent\"":2}}"",
    ""urlParams"": {
      ""id"": ""d2941460-13e4-43f1-8a13-23a36bc1a714""
    }
  },
  ""context"": {
    ""errors"": [
      {
        ""field"": ""deliveryMethod"",
        ""message"": ""Invalid deliveryMethod. Please specify a valid delivery method.""
      }
    ]
  }
}",
                HttpStatusCode.BadRequest,
                "deliveryMethod: Invalid deliveryMethod. Please specify a valid delivery method."
            ),

            // 401
            new TestCaseData(
                "{\"code\":401,\"message\":\"Invalid JWT Token\"}",
                HttpStatusCode.Unauthorized,
                "Invalid JWT Token"
            ),

            // 404
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\n    \""shopper\"": {\n        \""email\"": \""newshopper@email.tld\"",\n        \""firstName\"": \""newJohn\"",\n        \""lastName\"": \""newDoe\"",\n        \""title\"": \""newMr\"",\n        \""externalId\"": \""newext-1\"",\n        \""telephoneNumber\"": \""new+441234567890\"",\n        \""locale\"": \""fr_FR\""\n    },\n    \""billingAddress\"": {\n        \""line1\"": \""newHouse 1\"",\n        \""line2\"": \""new123 street\"",\n        \""postalCode\"": \""newAB1 2EF\"",\n        \""city\"": \""newHometown\"",\n        \""countryCode\"": \""FR\""\n    },\n    \""items\"": [\n        {\n            \""name\"": \""newLion King\"",\n            \""description\"": \""newOnline ticket sale The lion king\"",\n            \""quantity\"": 2,\n            \""amount\"": {\n                \""value\"": 3000,\n                \""currency\"": \""GBP\""\n            },\n            \""tax\"": {\n                \""value\"": 100,\n                \""currency\"": \""GBP\""\n            },\n            \""externalId\"": \""123\""\n        }   \n    ]\n}"",
    ""urlParams"": {
      ""id"": ""20c33214-c217-4398-927e-d089b4db06a6""
    }
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""Cannot find Order. Please specify a valid orderId.""
      }
    ]
  }
}",
                HttpStatusCode.NotFound,
                "Cannot find Order. Please specify a valid orderId."
            ),
        };

        #endregion

        #region Country

        public static IEnumerable<TestCaseData> GetUsStates_IfApiResponseSuccessful_ReturnsUsStates = new[]
        {
            new TestCaseData(
                @"{
  ""request"": [],
  ""response"": [
    {
      ""name"": ""Alabama"",
      ""abbreviation"": ""AL""
    },
    {
      ""name"": ""Alaska"",
      ""abbreviation"": ""AK""
    },
    {
      ""name"": ""Arizona"",
      ""abbreviation"": ""AZ""
    },
    {
      ""name"": ""West Virginia"",
      ""abbreviation"": ""WV""
    }
  ]
}",
                new List<CountryTerritorialUnit>
                {
                    new CountryTerritorialUnit
                    {
                        Name = "Alabama",
                        Abbreviation = "AL"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "Alaska",
                        Abbreviation = "AK"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "Arizona",
                        Abbreviation = "AZ"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "West Virginia",
                        Abbreviation = "WV"
                    },
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetCanadaProvinces_IfApiResponseSuccessful_ReturnsCanadaProvinces = new[]
        {
            new TestCaseData(
                @"{
  ""request"": [],
  ""response"": [
    {
      ""name"": ""Alberta"",
      ""abbreviation"": ""AB""
    },
    {
      ""name"": ""British Columbia"",
      ""abbreviation"": ""BC""
    },
    {
      ""name"": ""Manitoba"",
      ""abbreviation"": ""MB""
    },
    {
      ""name"": ""New Brunswick"",
      ""abbreviation"": ""NB""
    }
  ]
}",
                new List<CountryTerritorialUnit>
                {
                    new CountryTerritorialUnit
                    {
                        Name = "Alberta",
                        Abbreviation = "AB"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "British Columbia",
                        Abbreviation = "BC"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "Manitoba",
                        Abbreviation = "MB"
                    },
                    new CountryTerritorialUnit
                    {
                        Name = "New Brunswick",
                        Abbreviation = "NB"
                    },
                }
            ),
        };

        #endregion

        #region Payment

        public static IEnumerable<TestCaseData> CreateNewPayment_CallsApiWithRightParameters = new[]
        {
            new TestCaseData(
                new CreatePaymentRequest
                {
                    OrderId = "5b148b26-7e48-489e-8156-89534194f8a6",
                    Amount = new Amount
                    {
                        Value = 4200,
                        Currency = "GBP"
                    }
                },
                "{\"orderId\":\"5b148b26-7e48-489e-8156-89534194f8a6\",\"amount\":{\"value\":4200,\"currency\":\"GBP\",\"exchangeRate\":0.0},\"amountOriginal\":null}"
            ),
        };

        public static IEnumerable<TestCaseData> CreateNewPayment_IfApiResponseFailed_ThrowsApiException = new[]
        {
            // 400
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\n\t\""orderId\"": \""5b148b26-7e48-489e-8156-89534194f8a6\"",\n\t\""amount\"": {\n\t\t\""value\"": 4200,\n\t\t\""currency\"": \""GBP\""\n\t},\n\t\""amountOriginal\"": null\n}""
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""New payment for order with ID \""5b148b26-7e48-489e-8156-89534194f8a6\"" already exists.""
      }
    ]
  }
}",
                HttpStatusCode.BadRequest,
                "New payment for order with ID \"5b148b26-7e48-489e-8156-89534194f8a6\" already exists."
            ),

            // 404
            new TestCaseData(
                @"{
  ""request"": {
    ""body"": ""{\n\t\""orderId\"": \""ff40a916-e609-46a7-a57a-5eaee55940de\"",\n\t\""amount\"": {\n\t\t\""value\"": 4200,\n\t\t\""currency\"": \""GBP\""\n\t},\n\t\""amountOriginal\"": null\n}""
  },
  ""context"": {
    ""errors"": [
      {
        ""message"": ""Order with id \""ff40a916-e609-46a7-a57a-5eaee55940de\"" does not exist.""
      }
    ]
  }
}",
                HttpStatusCode.NotFound,
                "Order with id \"ff40a916-e609-46a7-a57a-5eaee55940de\" does not exist."
            ),
        };


        #endregion
    }
}
