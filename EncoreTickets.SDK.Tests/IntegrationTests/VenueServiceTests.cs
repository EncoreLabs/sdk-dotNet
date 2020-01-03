using System.Linq;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class VenueServiceTests
    {
        private IConfiguration configuration;
        private VenueServiceApi service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            var context = new ApiContext(Environments.Sandbox, configuration["Venue:Username"], configuration["Venue:Password"]);
            service = new VenueServiceApi(context, true);
        }

        [Test]
        public void Authentication_Successful()
        {
            service.AuthenticationService.Authenticate();

            Assert.True(service.AuthenticationService.IsThereAuthentication());
        }

        [Test]
        public void GetStandardAttributes_Successful()
        {
            var attributes = service.GetStandardAttributes();

            Assert.IsNotEmpty(attributes);
            foreach (var attribute in attributes)
            {
                Assert.NotNull(attribute.Title);
                Assert.NotNull(attribute.Intention);
            }
        }

        [Test]
        public void UpsertStandardAttributeByTitle_Successful()
        {
            var sourceAttribute = service.GetStandardAttributes().First();

            var updatedAttribute = service.UpsertStandardAttributeByTitle(sourceAttribute);

            AssertExtension.AreObjectsValuesEqual(sourceAttribute, updatedAttribute);
        }

        [Test]
        public void GetSeatAttributes_Successful()
        {
            var attributes = service.GetSeatAttributes(configuration["Venue:TestVenueIdWithSeatAttributes"]);

            Assert.IsNotEmpty(attributes);
            foreach (var attribute in attributes)
            {
                Assert.NotNull(attribute.SeatIdentifier);
                Assert.IsNotEmpty(attribute.Attributes);
                Assert.True(attribute.Attributes.All(a => !string.IsNullOrEmpty(a.Title)));
            }
        }

        [Test]
        public void UpsertSeatAttributes_Successful()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);

            var result = service.UpsertSeatAttributes(venueId, sourceAttributes);

            Assert.True(result);
        }

        [Test]
        public void GetAllVenues_Successful()
        {
            var venues = service.GetVenues();

            Assert.IsNotEmpty(venues);
            foreach (var venue in venues)
            {
                AssertVenueHasDetails(venue);
            }
        }

        [Test]
        public void GetVenueById_Successful()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            
            var venue = service.GetVenueById(venueId);

            AssertVenueHasDetails(venue);
        }

        [Test]
        public void UpdateVenue_Successful()
        {
            var venueId = configuration["Venue:TestVenueIdWithAddress"];
            var sourceVenue = service.GetVenueById(venueId);

            var updatedVenue = service.UpdateVenueById(sourceVenue);

            AssertVenueHasDetails(updatedVenue);
        }

        private void AssertVenueHasDetails(Venue.Models.Venue venue)
        {
            Assert.False(string.IsNullOrEmpty(venue.Title));
            Assert.False(string.IsNullOrEmpty(venue.InternalId));
            Assert.False(string.IsNullOrEmpty(venue.CompositeId));
        }
    }
}
