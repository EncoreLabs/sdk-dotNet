using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue
{
    internal class VenueServiceTests : VenueServiceApi
    {
        private MockersForApiService mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public VenueServiceTests() : base(new ApiContext())
        {
        }

        [TestCaseSource(typeof(VenueServiceTestsSource), nameof(VenueServiceTestsSource.GetVenues_IfApiResponseSuccessful_ReturnsVenues))]
        public void GetVenues_IfApiResponseSuccessful_ReturnsVenues(string responseContent,
            List<SDK.Venue.Models.Venue> expected)
        {
            mockers = new MockersForApiService();
            mockers.RestClientWrapperMock
                .Setup(x => x.Execute<VenuesResponse>(It.IsAny<IRestClient>(), It.IsAny<IRestRequest>()))
                .Returns((IRestClient client, IRestRequest request) =>
                    RestResponseFactory.GetSuccessJsonResponse<VenuesResponse>(client, request, responseContent));

            var result = GetVenues();

            AssertExtension.AreObjectsValuesEqual(expected, result);
            VerifyClientWrapperForGetVenues();
        }

        [Test]
        public void GetVenues_IfApiResponseFailed_ThrowsApiException()
        {
            mockers = new MockersForApiService();
            var code = HttpStatusCode.InternalServerError;
            var responseContent = ""; // pass real data
            mockers.RestClientWrapperMock
                .Setup(x => x.Execute<VenuesResponse>(It.IsAny<IRestClient>(), It.IsAny<IRestRequest>()))
                .Returns((IRestClient client, IRestRequest request) =>
                    RestResponseFactory.GetFailedJsonResponse<VenuesResponse>(client, request, responseContent, code));

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = GetVenues();
            });

            Assert.AreEqual(code, exception.ResponseCode);
            VerifyClientWrapperForGetVenues();
        }

        private void VerifyClientWrapperForGetVenues()
        {
            mockers.RestClientWrapperMock.Verify(
                x => x.Execute<VenuesResponse>(
                    It.Is<IRestClient>(client =>
                        client.BaseUrl.ToString() == BaseUrl
                    ),
                    It.Is<IRestRequest>(request =>
                        request.Method == Method.GET &&
                        request.Resource == "v1/venues" &&
                        request.RequestFormat == DataFormat.Json)
                ), Times.Once);
        }
    }

    public static class VenueServiceTestsSource
    {
        public static IEnumerable<TestCaseData> GetVenues_IfApiResponseSuccessful_ReturnsVenues = new []
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{}},\"response\":{\"results\":[{\"compositeId\":\"THEB00-VEN-LDN~12\",\"internalId\":\"12\",\"title\":\"test!!!\",\"address\":{\"firstLine\":\"Upper Street\",\"secondLine\":null,\"thirdLine\":null,\"city\":\"London\",\"postcode\":\"N1 2UD\",\"region\":{\"name\":\"LDN\",\"isoCode\":\"LDN\"},\"country\":{\"name\":\"Great Britain\",\"isoCode\":\"GBR\"},\"latitude\":\"51.541490\",\"longitude\":\"-0.102537\"},\"contentOverriddenAt\":\"2019-10-07T14:31:34+00:00\"},{\"compositeId\":\"THEB00-VEN-LDN~138\",\"internalId\":\"138\",\"title\":\"Apollo Victoria Theatre v3\",\"address\":{\"firstLine\":\"Rosebery Avenue\",\"secondLine\":null,\"thirdLine\":null,\"city\":\"London\",\"postcode\":\"EC1R 4TN\",\"region\":{\"name\":\"London\",\"isoCode\":\"LDN\"},\"country\":{\"name\":\"Great Britain\",\"isoCode\":\"GBR\"},\"latitude\":\"51.5294\",\"longitude\":\"-0.1062\"},\"contentOverriddenAt\":\"2019-09-03T14:13:28+00:00\"}]},\"context\":null}",
                new List<SDK.Venue.Models.Venue>
                {
                    new SDK.Venue.Models.Venue
                    {
                        compositeId = "THEB00-VEN-LDN~12",
                        internalId = "12",
                        title = "test!!!",
                        address = new Address
                        {
                            firstLine = "Upper Street",
                            secondLine = null,
                            thirdLine = null,
                            city = "London",
                            postcode = "N1 2UD",
                            region = new Region
                            {
                                name = "LDN",
                                isoCode = "LDN"
                            },
                            country = new Country
                            {
                                name = "Great Britain",
                                isoCode = "GBR"
                            },
                            latitude = "51.541490",
                            longitude = "-0.102537"
                        },
                        contentOverriddenAt = new DateTime(2019, 10, 7, 14, 31, 34)
                    },
                    new SDK.Venue.Models.Venue
                    {
                        compositeId = "THEB00-VEN-LDN~138",
                        internalId = "138",
                        title = "Apollo Victoria Theatre v3",
                        address = new Address
                        {
                            firstLine = "Rosebery Avenue",
                            secondLine = null,
                            thirdLine = null,
                            city = "London",
                            postcode = "EC1R 4TN",
                            region = new Region
                            {
                                name = "London",
                                isoCode = "LDN"
                            },
                            country = new Country
                            {
                                name = "Great Britain",
                                isoCode = "GBR"
                            },
                            latitude = "51.5294",
                            longitude = "-0.1062"
                        },
                        contentOverriddenAt = new DateTime(2019, 9, 3, 14, 13, 28)
                    },
                }
            ) {TestName = $"{nameof(GetVenues_IfApiResponseSuccessful_ReturnsVenues)}: API returns venues"},
        };
    }
}
