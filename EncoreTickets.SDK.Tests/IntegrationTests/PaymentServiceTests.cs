using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Payment;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;
using EncoreTickets.SDK.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    internal class PaymentServiceTests
    {
        private IConfiguration configuration;
        private Environments environment;
        private ApiContext context;
        private PaymentServiceApi service;
        private string username;
        private string password;
        private string apiKey;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            environment = Environments.Sandbox;
            username = configuration["Payment:Username"];
            password = configuration["Payment:Password"];
            apiKey = configuration["Payment:ApiKey"];
            context = new ApiContext(environment, apiKey)
            {
                Correlation = Guid.NewGuid().ToString(),
            };
            service = new PaymentServiceApi(context, true);
        }

        [Test]
        public void Authentication_IfUsingUsernameAndPassword_Successful()
        {
            context = new ApiContext(environment, username, password);
            service = new PaymentServiceApi(context, true);

            service.AuthenticationService.Authenticate();

            Assert.True(service.AuthenticationService.IsThereAuthentication());
        }

        [Test]
        public void GetOrder_Successful()
        {
            var channelId = configuration["Payment:TestChannelId"];
            var externalId = configuration["Payment:TestOrderExternalId"];

            var order = service.GetOrder(channelId, externalId);

            AssertThatOrderIsCorrect(order, channelId, externalId);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void GetOrder_IfOrderWithChannelAndExternalIdDoesNotExist_Exception404()
        {
            var channelId = "not_channel";
            var externalId = "not_id";

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.GetOrder(channelId, externalId);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void CreateOrder_Successful()
        {
            var channelId = configuration["Payment:TestChannelId"];
            var externalId = Guid.NewGuid().ToString();
            var amount = new Amount
            {
                Value = 8100,
                Currency = "USD",
                ExchangeRate = 1.2,
            };
            var orderRequest = new CreateOrderRequest
            {
                Description = "test description",
                ChannelId = channelId,
                ExternalId = externalId,
                RedirectUrl = "https://payment-service.qatixuk.io/redirect",
                Origin = "https://payment-service.qatixuk.io",
                BillingAddress = new Address
                {
                    Line1 = "115 Shaftesbury Avenue",
                    Line2 = null,
                    PostalCode = "WC2H 8AF",
                    City = "Cambridge Circus",
                    CountryCode = "UK",
                    LegacyCountryCode = null,
                    StateOrProvince = "London",
                },
                Shopper = new Shopper
                {
                    Email = "test@test.com",
                    TelephoneNumber = "02072578183",
                    Title = "MS",
                    FirstName = "INNA",
                    LastName = "IVANOVA",
                    ExternalId = null,
                },
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Name = "WICKED",
                        Description = null,
                        Quantity = 1,
                        Amount = amount,
                        Tax = null,
                        ExternalId = "1587",
                    },
                },
                Amount = amount,
                RiskData = new RiskData
                {
                    DaysToEvent = 0,
                    DeliveryMethod = "collection",
                    OfficeId = 1,
                },
            };

            var order = service.CreateOrder(orderRequest);

            AssertThatOrderIsCorrect(order, channelId, externalId);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void CreateOrder_IfOrderExists_Exception400()
        {
            var channelId = configuration["Payment:TestChannelId"];
            var externalId = configuration["Payment:TestCreateOrderExternalId"];
            var orderRequest = new CreateOrderRequest
            {
                Description = "test description",
                ChannelId = channelId,
                ExternalId = externalId,
                RedirectUrl = "https://payment-service.qatixuk.io/redirect",
                Origin = "https://payment-service.qatixuk.io",
                Amount = new Amount
                {
                    Value = 8100,
                    Currency = "USD",
                    ExchangeRate = 1.2,
                },
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.CreateOrder(orderRequest);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void UpdateOrder_Successful()
        {
            var channelId = configuration["Payment:TestChannelId"];
            var externalId = configuration["Payment:TestOrderExternalId"];
            var order = service.GetOrder(channelId, externalId);
            var newGuid = Guid.NewGuid();
            var updateOrderRequest = new UpdateOrderRequest
            {
                Shopper = order.Shopper,
                BillingAddress = order.BillingAddress,
                Items = order.Items,
                RiskData = order.RiskData,
            };
            updateOrderRequest.Shopper.FirstName = $"Tom{newGuid}";
            updateOrderRequest.BillingAddress.Line2 = $"Address{newGuid}";
            updateOrderRequest.Items.ForEach(x => x.Name = $"Name{newGuid}");
            updateOrderRequest.RiskData = new RiskData();

            var updatedOrder = service.UpdateOrder(order.Id, updateOrderRequest);

            AssertThatOrderIsCorrect(updatedOrder, channelId, externalId);
            updatedOrder.ShouldBeEquivalentToObjectWithMoreProperties(order);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void UpdateOrder_IfDeliveryMethodIsInvalid_Exception400()
        {
            var channelId = configuration["Payment:TestChannelId"];
            var externalId = configuration["Payment:TestOrderExternalId"];
            var order = service.GetOrder(channelId, externalId);
            var updateOrderRequest = new UpdateOrderRequest
            {
                Shopper = order.Shopper,
                BillingAddress = order.BillingAddress,
                Items = order.Items,
                RiskData = new RiskData
                {
                    DaysToEvent = 2,
                    DeliveryMethod = "invalid",
                    OfficeId = 1,
                },
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpdateOrder(order.Id, updateOrderRequest);
            });

            Assert.AreEqual(HttpStatusCode.BadRequest, exception.ResponseCode);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void UpdateOrder_IfOrderWithIdDoesNotExist_Exception404()
        {
            var orderId = "not_id";
            var updateOrderRequest = new UpdateOrderRequest();

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.UpdateOrder(orderId, updateOrderRequest);
            });

            Assert.AreEqual(HttpStatusCode.NotFound, exception.ResponseCode);
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        [Test]
        public void GetUsStates_Successful()
        {
            var states = service.GetUsStates();

            Assert.NotNull(states);
            states.ForEach(x =>
            {
                Assert.NotNull(x.Name);
                Assert.NotNull(x.Abbreviation);
            });
        }

        [Test]
        public void GetCanadaProvinces_Successful()
        {
            var states = service.GetCanadaProvinces();

            Assert.NotNull(states);
            states.ForEach(x =>
            {
                Assert.NotNull(x.Name);
                Assert.NotNull(x.Abbreviation);
            });
            Assert.AreEqual(context.Correlation, context.ReceivedCorrelation);
        }

        private void AssertThatOrderIsCorrect(Order order, string channelId, string externalId)
        {
            Assert.IsNotNull(order.Id);
            Assert.AreEqual(channelId, order.ChannelId);
            Assert.AreEqual(externalId, order.ExternalId);
        }
    }
}
