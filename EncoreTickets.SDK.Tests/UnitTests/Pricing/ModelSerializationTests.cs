using EncoreTickets.SDK.Pricing.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Serializers;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Pricing
{
    /// <summary>
    /// This class contains tests to verify that all properties of the DTO classes are named correctly.
    /// The source test data in this class should ALWAYS be in the same format as an actual API response.
    /// </summary>
    internal class ModelSerializationTests
    {
        private const string DateFormat = "yyyy-MM-ddTHH:mm:sszzz";
        private BaseJsonSerializer serializer;

        public ModelSerializationTests()
        {
            serializer = new DefaultJsonSerializer
            {
                DateFormat = DateFormat,
            };
        }

        [Test]
        public void ExchangeRate_CorrectlySerialized()
        {
            TestCorrectSerialization<ExchangeRate>(@"{
                ""id"": 2610,
                ""baseCurrency"": ""GBP"",
                ""targetCurrency"": ""USD"",
                ""rate"": 1.26715,
                ""encoreRate"": 1.26715,
                ""protectionMargin"": 0,
                ""datetimeOfSourcing"": ""2020-06-11T06:00:00+00:00"",
                ""createdAt"": ""2020-06-11T06:23:06+00:00"",
                ""updatedAt"": ""2020-06-11T06:23:06+00:00"",
                ""sourced"": 1
            }");
        }

        [Test]
        public void PriceBand_CorrectlySerialized()
        {
            TestCorrectSerialization<PriceBand>(@"{
                ""date"": ""2020-09-09T19:30:00+00:00"",
                ""displayCurrency"": ""USD"",
                ""createdAt"": ""2020-06-11T12:26:40+00:00"",
                ""percentage"": 0,
                ""offer"": false,
                ""noBookingFee"": false,
                ""salePrice"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 12800,
                        ""currency"": ""GBP""
                    },
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 16300,
                        ""currency"": ""USD""
                    }
                ],
                ""faceValue"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 9950,
                        ""currency"": ""GBP""
                    },
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 12700,
                        ""currency"": ""USD""
                    }
                ]
            }");
        }

        [Test]
        public void DailyPriceRange_SerializedCorrectly()
        {
            TestCorrectSerialization<DailyPriceRange>(@"{
                ""date"": ""2020-09-01T19:30:00+00:00"",
                ""displayCurrency"": ""GBP"",
                ""createdAt"": ""2020-06-11T12:43:53+00:00"",
                ""minPrice"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 3200,
                        ""currency"": ""GBP""
                    }
                ],
                ""maxPrice"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 12800,
                        ""currency"": ""GBP""
                    }
                ],
                ""offer"": false,
                ""includesBookingFee"": true
            }");
        }

        [Test]
        public void MonthlyPriceRange_SerializedCorrectly()
        {
            TestCorrectSerialization<MonthlyPriceRange>(@"{
                ""date"": {
                    ""month"": 9,
                    ""year"": 2020
                },
                ""displayCurrency"": ""GBP"",
                ""createdAt"": ""2020-06-11T12:46:55+00:00"",
                ""minPrice"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 3200,
                        ""currency"": ""GBP""
                    }
                ],
                ""maxPrice"": [
                    {
                        ""decimalPlaces"": 2,
                        ""value"": 22400,
                        ""currency"": ""GBP""
                    }
                ],
                ""offer"": false,
                ""includesBookingFee"": true
            }");
        }

        [Test]
        public void PriceRuleSummary_SerializedCorrectly()
        {
            TestCorrectSerialization<PriceRuleSummary>(@"{
                ""id"": 1,
                ""name"": ""1250 group rule"",
                ""active"": 1
            }");
        }

        [Test]
        public void PriceRule_SerializedCorrectly()
        {
            TestCorrectSerialization<PriceRule>(@"{
                ""id"": 1,
                ""name"": ""1250 group rule"",
                ""type"": 0,
                ""weight"": 10,
                ""active"": 1,
                ""createdAt"": ""2018-11-07T11:03:05+00:00"",
                ""updatedAt"": ""2018-11-07T11:03:05+00:00"",
                ""qualifiers"": [
                    {
                        ""id"": 1,
                        ""type"": ""partnerGroup"",
                        ""properties"": [
                            {
                                ""id"": 1,
                                ""name"": ""group"",
                                ""value"": ""1250"",
                                ""type"": ""partnerGroup"",
                                ""createdAt"": ""2018-11-07T11:03:05+00:00"",
                                ""updatedAt"": ""2018-11-07T11:03:05+00:00""
                            }
                        ],
                        ""createdAt"": ""2018-11-07T11:03:05+00:00"",
                        ""updatedAt"": ""2018-11-07T11:03:05+00:00"",
                        ""publishedAt"": ""2018-11-07T11:03:05+00:00""
                    }
                ],
                ""modifiers"": [
                    {
                        ""id"": 1,
                        ""mode"": 0,
                        ""adjustmentValue"": 12.5,
                        ""adjustmentType"": ""percentage"",
                        ""roundingPrecision"": 100,
                        ""roundingType"": ""up"",
                        ""weight"": 10,
                        ""createdAt"": ""2018-11-07T11:03:05+00:00"",
                        ""updatedAt"": ""2018-11-07T11:03:05+00:00""
                    }
                ]
            }");
        }

        [Test]
        public void PartnerGroup_SerializedCorrectly()
        {
            TestCorrectSerialization<PartnerGroup>(@"{
                ""id"": 3,
                ""name"": ""1000"",
                ""partners"": null
            }");
        }

        [Test]
        public void Partner_SerializedCorrectly()
        {
            TestCorrectSerialization<Partner>(@"{
                ""id"": 35,
                ""name"": """",
                ""officeId"": ""32"",
                ""currencyCode"": ""GBP"",
                ""defaultDisplayCurrencyCode"": ""GBP"",
                ""description"": ""Travel Trade 10%"",
                ""partnerGroup"": {
                    ""id"": 3,
                    ""name"": ""1000"",
                    ""partners"": [
                        null,
                        {
                            ""id"": 68,
                            ""name"": """",
                            ""officeId"": ""54"",
                            ""currencyCode"": ""USD"",
                            ""defaultDisplayCurrencyCode"": ""USD"",
                            ""description"": ""US Dollar Accounts"",
                            ""partnerGroup"": null
                        }
                    ]
                }
            }");
        }

        private void TestCorrectSerialization<T>(string json)
        {
            json = json.StripWhitespace();

            var deserialized = serializer.Deserialize<T>(json);
            var serialized = serializer.Serialize(deserialized).StripWhitespace();

            Assert.AreEqual(json, serialized);
        }
    }
}
