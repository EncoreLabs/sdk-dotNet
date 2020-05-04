using System;
using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Content;
using EncoreTickets.SDK.Content.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Tests.Helpers.ApiServiceMockers;
using EncoreTickets.SDK.Utilities.Exceptions;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Content
{
    internal class ContentServiceApiTests : ContentServiceApi
    {
        private const string TestProductValidId = "1";

        private MockersForApiService mockers;

        protected override ApiRequestExecutor Executor =>
            new ApiRequestExecutor(Context, BaseUrl, mockers.RestClientBuilderMock.Object);

        public ContentServiceApiTests() : base(new ApiContext(Environments.Sandbox))
        {
        }

        [SetUp]
        public void CreateMockers()
        {
            mockers = new MockersForApiService();
        }

        #region GetLocations
        
        [Test]
        public void GetLocations_CallsApiWithRightParameters()
        {
            mockers.SetupAnyExecution<ApiResponse<List<Location>>>();

            try
            {
                GetLocations();
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<List<Location>>>(BaseUrl, "v1/locations", Method.GET);
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetLocations_IfApiResponseSuccessful_ReturnsLocations))]
        public void GetLocations_IfApiResponseSuccessful_ReturnsLocations(
            string responseContent,
            List<Location> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<List<Location>>>(responseContent);

            var actual = GetLocations();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetLocations_IfApiResponseFailed_ThrowsApiException))]
        public void GetLocations_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code)
        {
            mockers.SetupFailedExecution<ApiResponse<List<Location>>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetLocations();
            });

            Assert.AreEqual(code, exception.ResponseCode);
        }

        #endregion

        #region GetProducts

        [Test]
        public void GetProducts_CallsApiWithRightParameters()
        {
            mockers.SetupAnyExecution<ApiResponse<List<Product>>>();

            try
            {
                GetProducts();
            }
            catch (Exception)
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<List<Product>>>(BaseUrl, "v1/products", Method.GET,
                expectedQueryParameters: new Dictionary<string, object> {{"page", 1}, {"limit", 1000}});
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetProducts_IfApiResponseSuccessful_ReturnsProducts))]
        public void GetProducts_IfApiResponseSuccessful_ReturnsProducts(
            string responseContent,
            List<Product> expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<List<Product>>>(responseContent);

            var actual = GetProducts();

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetProducts_IfApiResponseFailed_ThrowsApiException))]
        public void GetProducts_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code)
        {
            mockers.SetupFailedExecution<ApiResponse<List<Product>>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetProducts();
            });

            Assert.AreEqual(code, exception.ResponseCode);
        }

        #endregion

        #region GetProductById

        [TestCase(null)]
        [TestCase("")]
        public void GetProductById_IfIdIsNotSet_ThrowsArgumentException(string id)
        {
            Assert.Catch<ArgumentException>(() =>
            {
                GetProductById(id);
            });
        }

        [TestCase("163")]
        [TestCase("not_id")]
        public void GetProductById_IfIdIsSet_CallsApiWithRightParameters(string id)
        {
            mockers.SetupAnyExecution<ApiResponse<Product>>();

            try
            {
                GetProductById(id);
            }
            catch
            {
                // ignored
            }

            mockers.VerifyExecution<ApiResponse<Product>>(BaseUrl, $"v1/products/{id}", Method.GET);
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetProductById_IfApiResponseSuccessful_ReturnsProduct))]
        public void GetProductById_IfApiResponseSuccessful_ReturnsProduct(string responseContent,
            Product expected)
        {
            mockers.SetupSuccessfulExecution<ApiResponse<Product>>(responseContent);

            var actual = GetProductById(TestProductValidId);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(ContentServiceApiTestsSource), nameof(ContentServiceApiTestsSource.GetProductById_IfApiResponseFailed_ThrowsApiException))]
        public void GetProductById_IfApiResponseFailed_ThrowsApiException(
            string responseContent,
            HttpStatusCode code,
            string expectedMessage)
        {
            mockers.SetupFailedExecution<ApiResponse<Product>>(responseContent, code);

            var exception = Assert.Catch<ApiException>(() =>
            {
                var actual = GetProductById(TestProductValidId);
            });

            Assert.AreEqual(code, exception.ResponseCode);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        #endregion
    }

    public static class ContentServiceApiTestsSource
    {
        public static IEnumerable<TestCaseData> GetLocations_IfApiResponseSuccessful_ReturnsLocations = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{}},\"response\":[{\"name\":\"United States\",\"isoCode\":\"US\",\"subLocations\":[{\"name\":\"Illinois\",\"isoCode\":\"US_IL\",\"subLocations\":[{\"name\":\"Chicago\",\"isoCode\":null}]},{\"name\":\"Nevada\",\"isoCode\":\"US_NV\",\"subLocations\":[{\"name\":\"Las Vegas\",\"isoCode\":null}]},{\"name\":\"New York\",\"isoCode\":\"US_NY\",\"subLocations\":[{\"name\":\"New York\",\"isoCode\":null}]}]}],\"context\":null}",
                new List<Location>
                {
                    new Location
                    {
                        Name = "United States",
                        IsoCode = "US",
                        SubLocations = new List<Location>
                        {
                            new Location
                            {
                                Name = "Illinois",
                                IsoCode = "US_IL",
                                SubLocations = new List<Location>
                                {
                                    new Location
                                    {
                                        Name = "Chicago",
                                        IsoCode = null
                                    }
                                }
                            },
                            new Location
                            {
                                Name = "Nevada",
                                IsoCode = "US_NV",
                                SubLocations = new List<Location>
                                {
                                    new Location
                                    {
                                        Name = "Las Vegas",
                                        IsoCode = null
                                    }
                                }
                            },
                            new Location
                            {
                                Name = "New York",
                                IsoCode = "US_NY",
                                SubLocations = new List<Location>
                                {
                                    new Location
                                    {
                                        Name = "New York",
                                        IsoCode = null
                                    }
                                }
                            },
                        }
                    }
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetLocations_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                "",
                HttpStatusCode.InternalServerError
            ),
        };

        public static IEnumerable<TestCaseData> GetProducts_IfApiResponseSuccessful_ReturnsProducts = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{\"limit\":\"1000\",\"page\":\"1\"},\"urlParams\":{}},\"response\":[{\"id\":\"V010\",\"name\":\"\\u00a310.00 Gift Voucher\",\"areaCode\":null,\"showType\":null,\"venue\":null},{\"id\":\"V020\",\"name\":\"\\u00a320.00 Gift Voucher\",\"areaCode\":null,\"showType\":null,\"venue\":null},{\"id\":\"V050\",\"name\":\"\\u00a350.00 Gift Voucher\",\"areaCode\":null,\"showType\":null,\"venue\":null}],\"context\":null}",
                new List<Product>
                {
                    new Product
                    {
                        Id = "V010",
                        Name = "£10.00 Gift Voucher",
                        AreaCode = null,
                        ShowType = null,
                        Venue = null
                    },
                    new Product
                    {
                        Id = "V020",
                        Name = "£20.00 Gift Voucher",
                        AreaCode = null,
                        ShowType = null,
                        Venue = null
                    },
                    new Product
                    {
                        Id = "V050",
                        Name = "£50.00 Gift Voucher",
                        AreaCode = null,
                        ShowType = null,
                        Venue = null
                    },
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetProducts_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                "",
                HttpStatusCode.InternalServerError
            ),
        };

        public static IEnumerable<TestCaseData> GetProductById_IfApiResponseSuccessful_ReturnsProduct = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{\"productId\":\"1587\"}},\"response\":{\"id\":\"1587\",\"name\":\"Wicked\",\"areaCode\":null,\"showType\":{\"id\":null,\"type\":\"show\"},\"firstPreviewDate\":null,\"openingDate\":null,\"boOpensDate\":\"2019-12-11T00:00:00+0000\",\"boClosesDate\":\"2020-11-28T00:00:00+0000\",\"runTime\":null,\"fitMaximum\":10,\"rating\":null,\"synopsis\":\"<p>WICKED, the West End and Broadway musical sensation, is already the 9<sup>th</sup> longest running musical in London theatre history. Winner of over 100 major awards, including three Tony Awards, two Olivier Awards and ten theatregoer-voted WhatsOnStage Awards (winning &lsquo;Best West End Show&rsquo; on three separate occasions), the classic musical has now been seen by 10 million people in London alone.</p>\\n<p>&lsquo;Packed with wit, storming songs and beautiful costumes&rsquo; (The Guardian), WICKED imagines an ingenious backstory and future possibilities to the lives of L. Frank Baum&rsquo;s beloved characters from &lsquo;The Wonderful Wizard of Oz&rsquo; and reveals the decisions and events that shape the destinies of two unlikely University friends on their journey to becoming <em>Glinda The Good</em> and the <em>Wicked Witch of the West</em>.&nbsp;</p>\\n\",\"venue\":{\"id\":\"138\",\"name\":\"Apollo Victoria Theatre\",\"address\":{\"firstLine\":\"17 Wilton Road\",\"secondLine\":null,\"thirdLine\":null,\"city\":\"London\",\"postCode\":\"SW1V 1LG\",\"region\":{\"name\":null,\"isoCode\":\"LDN\"},\"country\":{\"name\":\"Great Britain\",\"isoCode\":\"GBR\"}}},\"onSale\":\"yes\",\"showFaceValue\":false},\"context\":null}",
                new Product
                {
                    Id = "1587",
                    Name = "Wicked",
                    AreaCode = null,
                    ShowType = new ShowType
                    {
                        Id = null,
                        Type = "show"
                    },
                    FirstPreviewDate = null,
                    OpeningDate = null,
                    BoOpensDate = new DateTime(2019, 12, 11),
                    BoClosesDate = new DateTime(2020, 11, 28),
                    RunTime = null,
                    FitMaximum = 10,
                    Rating = null,
                    Synopsis = "<p>WICKED, the West End and Broadway musical sensation, is already the 9<sup>th</sup> longest running musical in London theatre history. Winner of over 100 major awards, including three Tony Awards, two Olivier Awards and ten theatregoer-voted WhatsOnStage Awards (winning &lsquo;Best West End Show&rsquo; on three separate occasions), the classic musical has now been seen by 10 million people in London alone.</p>\n<p>&lsquo;Packed with wit, storming songs and beautiful costumes&rsquo; (The Guardian), WICKED imagines an ingenious backstory and future possibilities to the lives of L. Frank Baum&rsquo;s beloved characters from &lsquo;The Wonderful Wizard of Oz&rsquo; and reveals the decisions and events that shape the destinies of two unlikely University friends on their journey to becoming <em>Glinda The Good</em> and the <em>Wicked Witch of the West</em>.&nbsp;</p>\n",
                    Venue = new SDK.Content.Models.Venue
                    {
                        Id = "138",
                        Name = "Apollo Victoria Theatre",
                        Address = new Address
                        {
                            FirstLine = "17 Wilton Road",
                            SecondLine = null,
                            ThirdLine = null,
                            City = "London",
                            PostCode = "SW1V 1LG",
                            Region = new Region
                            {
                                Name = null,
                                IsoCode = "LDN"
                            },
                            Country = new Country
                            {
                                Name = "Great Britain",
                                IsoCode = "GBR"
                            }
                        }
                    },
                    OnSale = "yes",
                    ShowFaceValue = false
                }
            ),
        };

        public static IEnumerable<TestCaseData> GetProductById_IfApiResponseFailed_ThrowsApiException = new[]
        {
            new TestCaseData(
                "{\"request\":{\"body\":\"\",\"query\":{},\"urlParams\":{\"productId\":\"invalid\"}},\"response\":\"\",\"context\":{\"errors\":[{\"message\":\"Sorry, nothing was found\"}]}}",
                HttpStatusCode.NotFound,
                "Sorry, nothing was found"
            ),
        };
    }
}
