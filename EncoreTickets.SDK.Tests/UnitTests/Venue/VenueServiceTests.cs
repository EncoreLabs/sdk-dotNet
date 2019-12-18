using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using NUnit.Framework;
using RestSharp;
using Attribute = EncoreTickets.SDK.Venue.Models.Attribute;

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

        #region GetVenues

        [Test]
        public void GetVenues_CallsApiWithCorrectParameters()
        {
            mockers = new MockersForApiService();
            mockers.SetupAnyExecution<VenuesResponse>();

            try
            {
                GetVenues();
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<VenuesResponse>(BaseUrl, "v1/venues", Method.GET);
        }

        [TestCaseSource(typeof(VenueServiceTestsSource), nameof(VenueServiceTestsSource.GetVenues_IfApiResponseSuccessful_ReturnsVenues))]
        public void GetVenues_IfApiResponseSuccessful_ReturnsVenues(string responseContent,
            List<SDK.Venue.Models.Venue> expected)
        {
            mockers = new MockersForApiService();
            mockers.SetupSuccessfulExecution<VenuesResponse>(responseContent);

            var actual = GetVenues();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [Test]
        public void GetVenues_IfApiResponseFailed_ThrowsApiException()
        {
            mockers = new MockersForApiService();
            var code = HttpStatusCode.InternalServerError;
            var responseContent = ""; // pass real data
            mockers.SetupFailedExecution<VenuesResponse>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetVenues();
            });

            Assert.AreEqual(code, exception.ResponseCode);
        }

        #endregion

        #region UpsertSeatAttributes

        [TestCaseSource(typeof(VenueServiceTestsSource), nameof(VenueServiceTestsSource.UpsertSeatAttributes_IfApiResponseSuccessful_ReturnsTrue))]
        public void UpsertSeatAttributes_IfApiResponseSuccessful_ReturnsTrue(string venueId,
            IEnumerable<SeatAttribute> seatAttributes, string responseContent)
        {
            mockers = new MockersForApiService();
            mockers.SetupSuccessfulExecution<ApiResponse<List<string>>>(responseContent);

            var actual = UpsertSeatAttributes(venueId, seatAttributes);

            Assert.True(actual);
        }

        #endregion
    }

    public static class VenueServiceTestsSource
    {
        public static IEnumerable<TestCaseData> GetVenues_IfApiResponseSuccessful_ReturnsVenues = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{}},\"response\":{\"results\":[{\"compositeId\":\"THEB00-VEN-LDN~12\",\"internalId\":\"12\",\"title\":\"test!!!\",\"address\":{\"firstLine\":\"Upper Street\",\"secondLine\":null,\"thirdLine\":null,\"city\":\"London\",\"postcode\":\"N1 2UD\",\"region\":{\"name\":\"LDN\",\"isoCode\":\"LDN\"},\"country\":{\"name\":\"Great Britain\",\"isoCode\":\"GBR\"},\"latitude\":\"51.541490\",\"longitude\":\"-0.102537\"},\"contentOverriddenAt\":\"2019-10-07T14:31:34+00:00\"},{\"compositeId\":\"THEB00-VEN-LDN~138\",\"internalId\":\"138\",\"title\":\"Apollo Victoria Theatre v3\",\"address\":{\"firstLine\":\"Rosebery Avenue\",\"secondLine\":null,\"thirdLine\":null,\"city\":\"London\",\"postcode\":\"EC1R 4TN\",\"region\":{\"name\":\"London\",\"isoCode\":\"LDN\"},\"country\":{\"name\":\"Great Britain\",\"isoCode\":\"GBR\"},\"latitude\":\"51.5294\",\"longitude\":\"-0.1062\"},\"contentOverriddenAt\":\"2019-09-03T14:13:28+00:00\"}]},\"context\":null}",
                new List<SDK.Venue.Models.Venue>
                {
                    new SDK.Venue.Models.Venue
                    {
                        CompositeId = "THEB00-VEN-LDN~12",
                        InternalId = "12",
                        Title = "test!!!",
                        Address = new Address
                        {
                            FirstLine = "Upper Street",
                            SecondLine = null,
                            ThirdLine = null,
                            City = "London",
                            Postcode = "N1 2UD",
                            Region = new Region
                            {
                                Name = "LDN",
                                IsoCode = "LDN"
                            },
                            Country = new Country
                            {
                                Name = "Great Britain",
                                IsoCode = "GBR"
                            },
                            Latitude = "51.541490",
                            Longitude = "-0.102537"
                        },
                        ContentOverriddenAt = new DateTime(2019, 10, 7, 14, 31, 34)
                    },
                    new SDK.Venue.Models.Venue
                    {
                        CompositeId = "THEB00-VEN-LDN~138",
                        InternalId = "138",
                        Title = "Apollo Victoria Theatre v3",
                        Address = new Address
                        {
                            FirstLine = "Rosebery Avenue",
                            SecondLine = null,
                            ThirdLine = null,
                            City = "London",
                            Postcode = "EC1R 4TN",
                            Region = new Region
                            {
                                Name = "London",
                                IsoCode = "LDN"
                            },
                            Country = new Country
                            {
                                Name = "Great Britain",
                                IsoCode = "GBR"
                            },
                            Latitude = "51.5294",
                            Longitude = "-0.1062"
                        },
                        ContentOverriddenAt = new DateTime(2019, 9, 3, 14, 13, 28)
                    },
                }
            ) {TestName = $"{nameof(GetVenues_IfApiResponseSuccessful_ReturnsVenues)}"},
        };

        public static IEnumerable<TestCaseData> UpsertSeatAttributes_IfApiResponseSuccessful_ReturnsTrue = new[]
        {
            new TestCaseData(
                "163",
                new List<SeatAttribute>
                {
                    new SeatAttribute
                    {
                        SeatIdentifier = "STALLS-O2",
                        StartDate = "",
                        EndDate = "",
                        PerformanceTimes = new List<string>(),
                        Attributes = new List<Attribute>
                        {
                            new Attribute
                            {
                                Title = "RestrictedView",
                                Description = "Restricted view",
                                Intention = "negative"
                            },
                            new Attribute
                            {
                                Title = "PillarInView",
                                Description = "Pillar in view",
                                Intention = "negative"
                            },
                        }
                    },
                },
                "{\"request\":{\"body\":\"{\\\"seats\\\":[{\\\"seatIdentifier\\\":\\\"STALLS-O2\\\",\\\"startDate\\\":\\\"\\\",\\\"endDate\\\":\\\"\\\",\\\"performanceTimes\\\":[],\\\"attributes\\\":[{\\\"title\\\":\\\"RestrictedView\\\",\\\"description\\\":\\\"Restricted view\\\",\\\"intention\\\":\\\"negative\\\"}]}]}\",\"query\":{},\"urlParams\":{\"venueId\":\"163\"}},\"response\":\"Success\",\"context\":null}"
            ) {TestName = $"{nameof(UpsertSeatAttributes_IfApiResponseSuccessful_ReturnsTrue)}"},
        };
    }
}
