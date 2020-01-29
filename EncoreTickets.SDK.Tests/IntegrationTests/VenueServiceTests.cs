using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Venue;
using EncoreTickets.SDK.Venue.Models;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Attribute = EncoreTickets.SDK.Venue.Models.Attribute;

namespace EncoreTickets.SDK.Tests.IntegrationTests
{
    [TestFixture]
    internal class VenueServiceTests
    {
        private IConfiguration configuration;
        private ApiContext context;
        private VenueServiceApi service;

        [SetUp]
        public void SetupState()
        {
            configuration = ConfigurationHelper.GetConfiguration();
            context = new ApiContext(Environments.Sandbox, configuration["Venue:Username"], configuration["Venue:Password"]);
            service = new VenueServiceApi(context, true);
        }

        [Test]
        public void Authentication_Successful()
        {
            service.AuthenticationService.Authenticate();

            Assert.True(service.AuthenticationService.IsThereAuthentication());
        }

        [Test]
        public void Authentication_IfBadCredentials_Exception401()
        {
            var apiContext = new ApiContext(context.Environment, "admin", "invalid_password");
            service = new VenueServiceApi(apiContext);

            var exception = Assert.Catch<ApiException>(() =>
            {
                service.AuthenticationService.Authenticate();
            });

            AssertApiException(exception, HttpStatusCode.Unauthorized);
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
        public void GetVenueById_Exception404()
        {
            var venueId = configuration["Venue:TestVenueIdNotExisting"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var venue = service.GetVenueById(venueId);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        [Test]
        public void UpdateVenue_Successful()
        {
            var venueId = configuration["Venue:TestVenueIdWithAddress"];
            var sourceVenue = service.GetVenueById(venueId);

            var updatedVenue = service.UpdateVenueById(sourceVenue);

            AssertVenueHasDetails(updatedVenue);
        }

        [Test]
        public void UpdateVenue_IfVenueWithoutAddress_Exception400()
        {
            var venueId = configuration["Venue:TestVenueIdWithoutAddress"];
            var sourceVenue = service.GetVenueById(venueId);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedVenue = service.UpdateVenueById(sourceVenue);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpdateVenue_IfContextUnauthorized_Exception401()
        {
            var venueId = configuration["Venue:TestVenueIdWithAddress"];
            service = new VenueServiceApi(context);
            var sourceVenue = service.GetVenueById(venueId);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedVenue = service.UpdateVenueById(sourceVenue);
            });

            AssertApiException(exception, HttpStatusCode.Unauthorized);
        }

        [Test]
        public void UpdateVenue_IfTokenInvalid_Exception403()
        {
            var venueId = configuration["Venue:TestVenueIdWithAddress"];
            context.AccessToken = "invalid_token";
            service = new VenueServiceApi(context);
            var sourceVenue = service.GetVenueById(venueId);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedVenue = service.UpdateVenueById(sourceVenue);
            });

            AssertApiException(exception, HttpStatusCode.Forbidden);
        }

        [Test]
        public void UpdateVenue_IfVenueDoesNotExist_Exception404()
        {
            var venueId = configuration["Venue:TestVenueIdNotExisting"];
            var sourceVenue = new Venue.Models.Venue {InternalId = venueId};
            try
            {
                service.GetVenueById(venueId);
                Assert.Fail("Venue actually exists");
            }
            catch (Exception e)
            {
                //ignore
            }

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedVenue = service.UpdateVenueById(sourceVenue);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
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
        public void UpsertStandardAttributeByTitle_IfDescriptionIsNotSet_Exception400()
        {
            var sourceAttribute = new Attribute
            {
                Title = "test",
                Intention = Intention.Negative
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedAttribute = service.UpsertStandardAttributeByTitle(sourceAttribute);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertStandardAttributeByTitle_IfIntentionIsInvalid_Exception400()
        {
            var sourceAttribute = new Attribute
            {
                Title = "test",
                Description = "test description",
                Intention = (Intention)100
            };

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedAttribute = service.UpsertStandardAttributeByTitle(sourceAttribute);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertStandardAttributeByTitle_IfContextUnauthorized_Exception401()
        {
            var sourceAttribute = new Attribute {Title = "test"};
            service = new VenueServiceApi(context);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedAttribute = service.UpsertStandardAttributeByTitle(sourceAttribute);
            });

            AssertApiException(exception, HttpStatusCode.Unauthorized);
        }

        [Test]
        public void UpsertStandardAttributeByTitle_IfTokenInvalid_Exception403()
        {
            var sourceAttribute = new Attribute { Title = "test" };
            context.AccessToken = "invalid_token";
            service = new VenueServiceApi(context);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var updatedAttribute = service.UpsertStandardAttributeByTitle(sourceAttribute);
            });

            AssertApiException(exception, HttpStatusCode.Forbidden);
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
        public void GetSeatAttributes_IfVenueExistsButNotFound_Exception404()
        {
            var venueId = configuration["Venue:TestVenueIdWithoutSeatAttributesAndVenueNotFound"];

            var exception = Assert.Catch<ApiException>(() =>
            {
                var attributes = service.GetSeatAttributes(venueId);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
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
        public void UpsertSeatAttributes_IfSeatAttributeIsNotInitialized_Exception400()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            var newAttributes = new List<SeatDetailed>
            {
                new SeatDetailed
                {
                }
            };
            var attributes = newAttributes.Concat(sourceAttributes);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, attributes);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UpsertSeatAttributes_IfSeatAttributeHasAttributeWithoutTitle_Exception400(string title)
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            var newAttributes = new List<SeatDetailed>
            {
                new SeatDetailed
                {
                    SeatIdentifier = "DRESS_CIRCLE-G3",
                    Attributes = new List<Attribute>
                    {
                        new Attribute
                        {
                            Title = title,
                            Description = "test description",
                            Intention = Intention.Neutral
                        }
                    }
                }
            };
            var attributes = newAttributes.Concat(sourceAttributes);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, attributes);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [TestCase(null)]
        [TestCase("")]
        public void UpsertSeatAttributes_IfSeatAttributeHasAttributeWithoutDescription_Exception400(string description)
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            var newAttributes = new List<SeatDetailed>
            {
                new SeatDetailed
                {
                    SeatIdentifier = "DRESS_CIRCLE-G3",
                    Attributes = new List<Attribute>
                    {
                        new Attribute
                        {
                            Title = "test title",
                            Description = description,
                            Intention = Intention.Neutral
                        }
                    }
                }
            };
            var attributes = newAttributes.Concat(sourceAttributes);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, attributes);
            });

            AssertApiException(exception, HttpStatusCode.BadRequest);
        }

        [Test]
        public void UpsertSeatAttributes_IfSeatAttributeIsNotInitialized_Exception401()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            service = new VenueServiceApi(context);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, sourceAttributes);
            });

            AssertApiException(exception, HttpStatusCode.Unauthorized);
        }

        [Test]
        public void UpsertSeatAttributes_IfTokenInvalid_Exception403()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            context.AccessToken = "invalid_token";
            service = new VenueServiceApi(context);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, sourceAttributes);
            });

            AssertApiException(exception, HttpStatusCode.Forbidden);
        }

        [Test]
        public void UpsertSeatAttributes_IfVenueDoesNotExist_CreatesVenue()
        {
            var venueId = Guid.NewGuid().ToString();
            var sourceAttributes = new List<SeatDetailed>();

            var result = service.UpsertSeatAttributes(venueId, sourceAttributes);

            Assert.True(result);
        }

        [Test]
        public void UpsertSeatAttributes_IfSeatAttributeHasNotStandardAndNotOtherName_Exception404()
        {
            var venueId = configuration["Venue:TestVenueIdWithSeatAttributes"];
            var sourceAttributes = service.GetSeatAttributes(venueId);
            var newAttributes = new List<SeatDetailed>
            {
                new SeatDetailed
                {
                    SeatIdentifier = "DRESS_CIRCLE-G3",
                    Attributes = new List<Attribute>
                    {
                        new Attribute
                        {
                            Title = "test_not_existing_title",
                            Description = "test description",
                            Intention = Intention.Neutral
                        }
                    }
                }
            };
            var attributes = newAttributes.Concat(sourceAttributes);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var result = service.UpsertSeatAttributes(venueId, attributes);
            });

            AssertApiException(exception, HttpStatusCode.NotFound);
        }

        private void AssertVenueHasDetails(Venue.Models.Venue venue)
        {
            Assert.False(string.IsNullOrEmpty(venue.Title));
            Assert.False(string.IsNullOrEmpty(venue.InternalId));
            Assert.False(string.IsNullOrEmpty(venue.CompositeId));
        }

        private void AssertApiException(ApiException exception, HttpStatusCode code)
        {
            Assert.AreEqual(code, exception.ResponseCode);
        }
    }
}
