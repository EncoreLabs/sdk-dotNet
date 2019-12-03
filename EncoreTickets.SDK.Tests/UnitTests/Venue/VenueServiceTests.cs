using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.RequestModels;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue
{
    internal class VenueServiceTests : VenueServiceApi
    {
        private static Mock<ApiRequestExecutor> executorMock;

        protected override ApiRequestExecutor Executor => executorMock.Object;

        [SetUp]
        public static void SetUp()
        {
            executorMock = new Mock<ApiRequestExecutor>(It.IsAny<ApiContext>(), It.IsAny<string>());
        }

        public VenueServiceTests() : base(new ApiContext())
        {
        }

        [Test]
        public void Venue_VenueServiceApi_Constructor_InitializesAll()
        {
            Assert.AreEqual(AuthenticationMethod.JWT, Context.AuthenticationMethod);
            Assert.IsNotNull(AuthenticationService);
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenues_IfSuccess_ReturnsVenues()
        {
            var venues = new List<SDK.Venue.Models.Venue> {new SDK.Venue.Models.Venue(), new SDK.Venue.Models.Venue()};
            executorMock
                .Setup(x => x
                    .ExecuteApiWithWrappedResponse<List<SDK.Venue.Models.Venue>, VenuesResponse, VenuesResponseContent>(
                        It.IsAny<string>(),
                        It.IsAny<RequestMethod>(),
                        null,
                        null,
                        null,
                        true))
                .Returns(() => new ApiResult<List<SDK.Venue.Models.Venue>>(
                    venues,
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = GetVenues();

            executorMock.Verify(mock =>
                mock.ExecuteApiWithWrappedResponse<List<SDK.Venue.Models.Venue>, VenuesResponse, VenuesResponseContent>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true), Times.Once);
            AssertExtension.EnumerableAreEquals(venues, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenues_IfNotSuccess_ThrowsApiException()
        {
            var venues = new List<SDK.Venue.Models.Venue> { new SDK.Venue.Models.Venue(), new SDK.Venue.Models.Venue() };
            executorMock
                .Setup(x => x
                    .ExecuteApiWithWrappedResponse<List<SDK.Venue.Models.Venue>, VenuesResponse, VenuesResponseContent>(
                        It.IsAny<string>(),
                        It.IsAny<RequestMethod>(),
                        null,
                        null,
                        null,
                        true))
                .Returns(() => new ApiResult<List<SDK.Venue.Models.Venue>>(
                    venues,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = GetVenues();
            });

            executorMock.Verify(mock =>
                mock.ExecuteApiWithWrappedResponse<List<SDK.Venue.Models.Venue>, VenuesResponse, VenuesResponseContent>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenueById_IfSuccess_Works()
        {
            const string venueId = "3456";
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<SDK.Venue.Models.Venue>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<SDK.Venue.Models.Venue>(
                    new SDK.Venue.Models.Venue(),
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = GetVenueById(venueId);

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<SDK.Venue.Models.Venue>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenueById_IfNotSuccess_ThrowsApiException()
        {
            const string venueId = "3456";
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<SDK.Venue.Models.Venue>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<SDK.Venue.Models.Venue>(
                    null,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = GetVenueById(venueId);
            });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<SDK.Venue.Models.Venue>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenueId_IfSuccess_Works()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<SeatAttribute>>(
                    seatAttributes,
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = GetSeatAttributes(venueId);

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
            AssertExtension.EnumerableAreEquals(seatAttributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenueId_IfNotSuccess_ThrowsApiException()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<SeatAttribute>>(
                    seatAttributes,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = GetSeatAttributes(venueId);
            });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenue_IfSuccess_Works()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<SeatAttribute>>(
                    seatAttributes,
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = GetSeatAttributes(new SDK.Venue.Models.Venue { internalId = venueId });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
            AssertExtension.EnumerableAreEquals(seatAttributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenue_IfNotSuccess_ThrowsApiException()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<SeatAttribute>>(
                    seatAttributes,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = GetSeatAttributes(new SDK.Venue.Models.Venue { internalId = venueId });
            });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<SeatAttribute>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetStandardAttributes_IfSuccess_Works()
        {
            var attributes = new List<StandardAttribute> { new StandardAttribute(), new StandardAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<StandardAttribute>>(
                    attributes,
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = GetStandardAttributes();

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                It.IsAny<string>(),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
            AssertExtension.EnumerableAreEquals(attributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetStandardAttributes_IfNotSuccess_ThrowsApiException()
        {
            var attributes = new List<StandardAttribute> { new StandardAttribute(), new StandardAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    null,
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<List<StandardAttribute>>(
                    attributes,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = GetStandardAttributes();
            });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<List<StandardAttribute>>(
                It.IsAny<string>(),
                It.IsAny<RequestMethod>(),
                null,
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertStandardAttributeByTitle_IfSuccess_ReturnsUpdated()
        {
            var attribute = new StandardAttribute { description = "desc", title = "title", intention = "" };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<StandardAttribute>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<StandardAttribute>(
                    attribute,
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = UpsertStandardAttributeByTitle(attribute);

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<StandardAttribute>(
                It.IsAny<string>(),
                It.IsAny<RequestMethod>(),
                It.IsAny<object>(),
                null,
                null,
                true), Times.Once);
            AssertExtension.SimplePropertyValuesAreEquals(attribute, result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertStandardAttributeByTitle_IfNotSuccess_ThrowsApiException()
        {
            var attribute = new StandardAttribute { description = "desc", title = "title", intention = "" };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<StandardAttribute>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<StandardAttribute>(
                    attribute,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = UpsertStandardAttributeByTitle(attribute);
            });
            
            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<StandardAttribute>(
                It.IsAny<string>(),
                It.IsAny<RequestMethod>(),
                It.IsAny<object>(),
                null,
                null,
                true), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfSuccess_ReturnsTrue()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<IEnumerable<string>>(
                    new[] { "Success" },
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = UpsertSeatAttributes(venueId, seatAttributes);

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)),
                null,
                null,
                true), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfNotSuccessDueToApi_ReturnsFalse()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<IEnumerable<string>>(
                    new[] { "" },
                    TestHelper.GetSuccessResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            var result = UpsertSeatAttributes(venueId, seatAttributes);

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)),
                null,
                null,
                true), Times.Once);
            Assert.IsFalse(result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfNotSuccessDueToHttp_ThrowsApiException()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                    It.IsAny<string>(),
                    It.IsAny<RequestMethod>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    true))
                .Returns(() => new ApiResult<IEnumerable<string>>(
                    null,
                    TestHelper.GetFailedResponse(),
                    It.IsAny<ApiContext>(),
                    It.IsAny<Context>(),
                    It.IsAny<Request>()));

            Assert.Catch<ApiException>(() =>
            {
                var result = UpsertSeatAttributes(venueId, seatAttributes);
            });

            executorMock.Verify(mock => mock.ExecuteApiWithWrappedResponse<IEnumerable<string>>(
                It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)),
                null,
                null,
                true), Times.Once);
        }
    }
}
