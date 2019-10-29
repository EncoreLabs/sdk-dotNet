using System.Collections.Generic;
using System.Linq;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Helpers;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using EncoreTickets.SDK.Venue.Models.RequestModels;
using EncoreTickets.SDK.Venue.Models.ResponseModels;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests.Venue
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
            var venues = new List<SDK.Venue.Models.Venue>{new SDK.Venue.Models.Venue(), new SDK.Venue.Models.Venue()};
            var venueResponse = new VenuesResponse {response = new Response {results = venues}};
            executorMock
                .Setup(x => x.ExecuteApiList<VenuesResponse>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<VenuesResponse>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<VenuesResponse>(venueResponse)));

            var result = GetVenues();
            executorMock.Verify(mock => mock.ExecuteApiList<VenuesResponse>(It.IsAny<string>(),
                    It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            AssertExtension.EnumerableAreEquals(venues, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenues_IfNotSuccess_ReturnsEmptyList()
        {
            var venueResponse = new VenuesResponse { response = new Response { results = null } };
            executorMock
                .Setup(x => x.ExecuteApiList<VenuesResponse>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<VenuesResponse>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<VenuesResponse>(venueResponse)));

            var result = GetVenues();
            executorMock.Verify(mock => mock.ExecuteApiList<VenuesResponse>(It.IsAny<string>(),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenueById_IfSuccess_Works()
        {
            const string venueId = "3456";
            executorMock
                .Setup(x => x.ExecuteApi<SDK.Venue.Models.Venue>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResult<SDK.Venue.Models.Venue>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<SDK.Venue.Models.Venue>(new SDK.Venue.Models.Venue())));

            var result = GetVenueById(venueId);
            executorMock.Verify(mock => mock.ExecuteApi<SDK.Venue.Models.Venue>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
        }

        [Test]
        public void Venue_VenueServiceApi_GetVenueById_IfNotSuccess_ReturnsNull()
        {
            const string venueId = "3456";
            executorMock
                .Setup(x => x.ExecuteApi<SDK.Venue.Models.Venue>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResult<SDK.Venue.Models.Venue>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<SDK.Venue.Models.Venue>(null)));

            var result = GetVenueById(venueId);
            executorMock.Verify(mock => mock.ExecuteApi<SDK.Venue.Models.Venue>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            Assert.IsNull(result);
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenueId_IfSuccess_Works()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<SeatAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<SeatAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<List<SeatAttribute>>(seatAttributes)));

            var result = GetSeatAttributes(venueId);
            executorMock.Verify(mock => mock.ExecuteApiList<List<SeatAttribute>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            AssertExtension.EnumerableAreEquals(seatAttributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenueId_IfNotSuccess_ReturnsEmpty()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<SeatAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<SeatAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<List<SeatAttribute>>(seatAttributes)));

            var result = GetSeatAttributes(venueId);
            executorMock.Verify(mock => mock.ExecuteApiList<List<SeatAttribute>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenue_IfSuccess_Works()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<SeatAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<SeatAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<List<SeatAttribute>>(seatAttributes)));

            var result = GetSeatAttributes(new SDK.Venue.Models.Venue{internalId = venueId});
            executorMock.Verify(mock => mock.ExecuteApiList<List<SeatAttribute>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            AssertExtension.EnumerableAreEquals(seatAttributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetSeatAttributesByVenue_IfNotSuccess_ReturnsEmpty()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<SeatAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<SeatAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<List<SeatAttribute>>(seatAttributes)));

            var result = GetSeatAttributes(new SDK.Venue.Models.Venue { internalId = venueId });
            executorMock.Verify(mock => mock.ExecuteApiList<List<SeatAttribute>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Venue_VenueServiceApi_GetStandardAttributes_IfSuccess_Works()
        {
            var attributes = new List<StandardAttribute> { new StandardAttribute(), new StandardAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<StandardAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<StandardAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<List<StandardAttribute>>(attributes)));

            var result = GetStandardAttributes();
            executorMock.Verify(mock => mock.ExecuteApiList<List<StandardAttribute>>(It.IsAny<string>(),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            AssertExtension.EnumerableAreEquals(attributes, result.ToList());
        }

        [Test]
        public void Venue_VenueServiceApi_GetStandardAttributes_IfNotSuccess_ReturnsEmpty()
        {
            var attributes = new List<StandardAttribute> { new StandardAttribute(), new StandardAttribute() };
            executorMock
                .Setup(x => x.ExecuteApiList<List<StandardAttribute>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), null, null))
                .Returns(() => new ApiResultList<List<StandardAttribute>>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<List<StandardAttribute>>(attributes)));

            var result = GetStandardAttributes();
            executorMock.Verify(mock => mock.ExecuteApiList<List<StandardAttribute>>(It.IsAny<string>(),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), null, null), Times.Once);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertStandardAttributeByTitle_IfSuccess_ReturnsUpdated()
        {
            var attribute = new StandardAttribute{description = "desc", title = "title", intention = ""};
            executorMock
                .Setup(x => x.ExecuteApi<StandardAttribute>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), attribute, null))
                .Returns(() => new ApiResult<StandardAttribute>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<StandardAttribute>(attribute)));

            var result = UpsertStandardAttributeByTitle(attribute);
            executorMock.Verify(mock => mock.ExecuteApi<StandardAttribute>(It.IsAny<string>(),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), attribute, null), Times.Once);
            AssertExtension.SimplePropertyValuesAreEquals(attribute, result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertStandardAttributeByTitle_IfNotSuccess_ReturnsNull()
        {
            var attribute = new StandardAttribute { description = "desc", title = "title", intention = "" };
            executorMock
                .Setup(x => x.ExecuteApi<StandardAttribute>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), attribute, null))
                .Returns(() => new ApiResult<StandardAttribute>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<StandardAttribute>(attribute)));

            var result = UpsertStandardAttributeByTitle(attribute);
            executorMock.Verify(mock => mock.ExecuteApi<StandardAttribute>(It.IsAny<string>(),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(), attribute, null), Times.Once);
            Assert.IsNull(result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfSuccess_ReturnsTrue()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> {new SeatAttribute(), new SeatAttribute()};
            executorMock
                .Setup(x => x.ExecuteApi<IEnumerable<string>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), It.IsAny<object>(), null))
                .Returns(() => new ApiResult<IEnumerable<string>>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<IEnumerable<string>>(new []{"Success"})));

            var result = UpsertSeatAttributes(venueId, seatAttributes);
            executorMock.Verify(mock => mock.ExecuteApi<IEnumerable<string>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)), null), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfNotSuccessDueToHttp_ReturnsFalse()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApi<IEnumerable<string>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), It.IsAny<object>(), null))
                .Returns(() => new ApiResult<IEnumerable<string>>(It.IsAny<ApiContext>(),
                    TestHelper.GetFailedResponse(), new ApiResponse<IEnumerable<string>>(new[] { "Success" })));

            var result = UpsertSeatAttributes(venueId, seatAttributes);
            executorMock.Verify(mock => mock.ExecuteApi<IEnumerable<string>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)), null), Times.Once);
            Assert.IsFalse(result);
        }

        [Test]
        public void Venue_VenueServiceApi_UpsertSeatAttributes_IfNotSuccessDueToApi_ReturnsFalse()
        {
            const string venueId = "3456";
            var seatAttributes = new List<SeatAttribute> { new SeatAttribute(), new SeatAttribute() };
            executorMock
                .Setup(x => x.ExecuteApi<IEnumerable<string>>(It.IsAny<string>(), It.IsAny<RequestMethod>(),
                    It.IsAny<bool>(), It.IsAny<object>(), null))
                .Returns(() => new ApiResult<IEnumerable<string>>(It.IsAny<ApiContext>(),
                    TestHelper.GetSuccessResponse(), new ApiResponse<IEnumerable<string>>(new[] { ""})));

            var result = UpsertSeatAttributes(venueId, seatAttributes);
            executorMock.Verify(mock => mock.ExecuteApi<IEnumerable<string>>(It.Is<string>(x => x.Contains(venueId)),
                It.IsAny<RequestMethod>(), It.IsAny<bool>(),
                It.Is<SeatAttributesRequest>(x => Equals(x.seats, seatAttributes)), null), Times.Once);
            Assert.IsFalse(result);
        }
    }
}
