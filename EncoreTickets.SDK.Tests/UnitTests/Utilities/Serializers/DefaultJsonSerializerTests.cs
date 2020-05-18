using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Venue.Models;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Serializers
{
    internal class DefaultJsonSerializerTests
    {
        [TestCaseSource(typeof(DefaultJsonSerializerTestsSource), nameof(DefaultJsonSerializerTestsSource.Serialize_ReturnsCorrectJson))]
        public void Serialize_ReturnsCorrectJson<T>(string dateFormat, NamingStrategy enumNamingStrategy, T item, string expected)
        {
            var serializer = new DefaultJsonSerializer(enumNamingStrategy) {DateFormat = dateFormat};

            var actual = serializer.Serialize(item);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(DefaultJsonSerializerTestsSource), nameof(DefaultJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserialized_ReturnsCorrectObject))]
        public void DeserializeRestResponse_IfJsonCanBeDeserialized_ReturnsCorrectObject<T>(string dateFormat, string json, T expected)
        {
            var response = new RestResponse<T> {Content = json};
            var serializer = new DefaultJsonSerializer { DateFormat = dateFormat };

            var actual = serializer.Deserialize<T>(response);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(DefaultJsonSerializerTestsSource), nameof(DefaultJsonSerializerTestsSource.DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException))]
        public void DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException<T>(string dateFormat, string json, T defaultWithDeserializedType)
        {
            var response = new RestResponse<T> { Content = json };
            var serializer = new DefaultJsonSerializer { DateFormat = dateFormat };

            Assert.Catch(() =>
            {
                var actual = serializer.Deserialize<T>(response);
            });
        }

        [TestCaseSource(typeof(DefaultJsonSerializerTestsSource), nameof(DefaultJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserialized_ReturnsCorrectObject))]
        public void DeserializeString_IfJsonCanBeDeserialized_ReturnsCorrectObject<T>(string dateFormat, string json, T expected)
        {
            var serializer = new DefaultJsonSerializer { DateFormat = dateFormat };

            var actual = serializer.Deserialize<T>(json);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(DefaultJsonSerializerTestsSource), nameof(DefaultJsonSerializerTestsSource.DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException))]
        public void DeserializeString_IfJsonCannotBeDeserialized_ThrowsException<T>(string dateFormat, string json, T defaultWithDeserializedType)
        {
            var serializer = new DefaultJsonSerializer { DateFormat = dateFormat };

            Assert.Catch(() =>
            {
                var actual = serializer.Deserialize<T>(json);
            });
        }
    }

    public static class DefaultJsonSerializerTestsSource
    {
        public static IEnumerable<TestCaseData> Serialize_ReturnsCorrectJson = new[]
        {
            new TestCaseData(
                null,
                null,
                1,
                "1"
            ),
            new TestCaseData(
                null,
                null,
                -18901,
                "-18901"
            ),
            new TestCaseData(
                null,
                null,
                4.1234567890,
                "4.123456789"
            ),
            new TestCaseData(
                null,
                null,
                4M,
                "4.0"
            ),
            new TestCaseData(
                null,
                null,
                "test string",
                "\"test string\""
            ),
            new TestCaseData(
                null,
                null,
                true,
                "true"
            ),
            new TestCaseData(
                null,
                null,
                new object(),
                "{}"
            ),
            new TestCaseData(
                null,
                null,
                new List<object>(),
                "[]"
            ),
            new TestCaseData(
                null,
                null,
                new List<string>(),
                "[]"
            ),
            new TestCaseData(
                null,
                null,
                new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9},
                "[1,2,3,4,5,6,7,8,9]"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    Number = 100,
                    Double = 9.087,
                    str = "test string"
                },
                "{\"number\":100,\"double\":9.087,\"str\":\"test string\"}"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    SomeList = new List<object>
                    {
                        new
                        {
                            someObject = new object(),
                            Number = 100
                        },
                        new
                        {
                            someString = "string"
                        }
                    }
                },
                "{\"someList\":[{\"someObject\":{},\"number\":100},{\"someString\":\"string\"}]}"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    DOUBLE = true
                },
                "{\"double\":true}"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    dEcImAl = 0.2456M
                },
                "{\"dEcImAl\":0.2456}"
            ),
            new TestCaseData(
                null,
                null,
                new DateTime(2020, 12, 31, 23, 59, 59),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                null,
                null,
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                null,
                null,
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Unspecified),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                null,
                new DateTime(2020, 10, 31, 16, 55, 20, DateTimeKind.Utc),
                "\"2020-10-31T16:55:20+00:00\""
            ),
            new TestCaseData(
                "yyyy-MM-dd",
                null,
                new DateTime(2020, 10, 31, 23, 59, 59, DateTimeKind.Utc),
                "\"2020-10-31\""
            ),
            new TestCaseData(
                null,
                null,
                Environments.Production,
                "\"production\""
            ),
            new TestCaseData(
                null,
                new DefaultNamingStrategy(),
                Environments.Production,
                "\"Production\""
            ),
            new TestCaseData(
                null,
                null,
                (Environments) 250,
                "250"
            ),
            new TestCaseData(
                null,
                new DefaultNamingStrategy(),
                (Environments) 250,
                "250"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    Enum = Environments.QA
                },
                "{\"enum\":\"qa\"}"
            ),
            new TestCaseData(
                null,
                new DefaultNamingStrategy(),
                new
                {
                    Enum = Environments.QA
                },
                "{\"enum\":\"QA\"}"
            ),
            new TestCaseData(
                null,
                null,
                new
                {
                    Intention = Intention.Positive
                },
                "{\"intention\":\"positive\"}"
            ),
        };

        public static IEnumerable<TestCaseData> Deserialize_IfJsonCanBeDeserialized_ReturnsCorrectObject = new[]
        {
            new TestCaseData(
                null,
                "1",
                1
            ),
            new TestCaseData(
                null,
                "-18901",
                -18901
            ),
            new TestCaseData(
                null,
                "4.123456789",
                4.1234567890
            ),
            new TestCaseData(
                null,
                "4.0",
                4M
            ),
            new TestCaseData(
                null,
                "4.0",
                4D
            ),
            new TestCaseData(
                null,
                "\"test string\"",
                "test string"
            ),
            new TestCaseData(
                null,
                "true",
                true
            ),
            new TestCaseData(
                null,
                "\"false\"",
                false
            ),
            new TestCaseData(
                null,
                "{}",
                new object()
            ),
            new TestCaseData(
                null,
                "[]",
                new List<object>()
            ),
            new TestCaseData(
                null,
                "[]",
                new List<string>()
            ),
            new TestCaseData(
                null,
                "[,,,]",
                new List<object>{null, null, null}
            ),
            new TestCaseData(
                null,
                "[1,2,3,4,5,6,7,8,]",
                new List<int>{1,2,3,4,5,6,7,8}
            ),
            new TestCaseData(
                null,
                "[1,2,3,4,5,6,7,8,9]",
                new List<double>{1,2,3,4,5,6,7,8,9}
            ),
            new TestCaseData(
                null,
                "[1,2,3]",
                new List<string>{"1", "2", "3"}
            ),
            new TestCaseData(
                null,
                "[1,2,3,4,5,6,7,8,9]",
                new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9}
            ),
            new TestCaseData(
                null,
                "{\"number\":100,\"doubl\":9.087,\"str\":\"test string\"}",
                new
                {
                    number = 100,
                    doubl = 9.087,
                    str = "test string"
                }
            ),
            new TestCaseData(
                null,
                "{\"number\":100,\"doubl\":9.087,\"str\":\"test string\"}",
                new
                {
                    number = 100,
                    doubl = 9.087,
                    anotherNumber = default(int)
                }
            ),
            new TestCaseData(
                null,
                "{\"someList\":[{\"someObject\":{},\"number\":100},{\"someString\":\"string\"}]}",
                new
                {
                    SomeList = new List<object>
                    {
                        new object(),
                        new object()
                    }
                }
            ),
            new TestCaseData(
                null,
                "{\"double\":true}",
                new
                {
                    DOUBLE = true
                }
            ),
            new TestCaseData(
                null,
                "{\"dEcImAl\":0.2456}",
                new
                {
                    dEcImAl = 0.2456M
                }
            ),
            new TestCaseData(
                null,
                "\"2020-12-31T23:59:59Z\"",
                new DateTime(2020, 12, 31, 23, 59, 59)
            ),
            new TestCaseData(
                null,
                "\"2020-12-31T23:59:59Z\"",
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Utc)
            ),
            new TestCaseData(
                null,
                "\"2020-12-31T23:59:59Z\"",
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Unspecified)
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                "\"2020-10-31T16:55:20+00:00\"",
                new DateTime(2020, 10, 31, 16, 55, 20, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                "\"2020-10-31\"",
                new DateTime(2020, 10, 31, 0,0,0, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                "\"2020-10-31T16:55:20+00:00\"",
                new DateTime(2020, 10, 31, 16, 55, 20, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "yyyy/MM/dd",
                "\"2020/10/11\"",
                new DateTime(2020, 10, 11, 0,0,0, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "yyyy/dd/MM",
                "\"2020/10/11\"",
                new DateTime(2020, 11, 10, 0,0,0, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "yyyy/MM/dd",
                "\"2020-10-31\"",
                new DateTime(2020, 10, 31, 0,0,0, DateTimeKind.Utc)
            ),
            new TestCaseData(
                "dd/MM/yyyy",
                "\"2020-10-31\"",
                new DateTime(2020, 10, 31, 0,0,0, DateTimeKind.Utc)
            ),
            new TestCaseData(
                null,
                "\"production\"",
                Environments.Production
            ),
            new TestCaseData(
                null,
                "250",
                (Environments) 250
            ),
            new TestCaseData(
                null,
                "{\"enum\":\"qa\"}",
                new
                {
                    Enum = Environments.QA
                }
            ),
            new TestCaseData(
                null,
                "{\"intention\":\"positive\"}",
                new
                {
                    Intention = Intention.Positive
                }
            ),
            new TestCaseData(
                null,
                "{\"intention\":1000}",
                new
                {
                    Intention = (Intention)1000
                }
            ),
        };

        public static IEnumerable<TestCaseData> DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException = new[]
        {
            new TestCaseData(
                null,
                "{",
                new object()
            ),
            new TestCaseData(
                null,
                "{}",
                default(int)
            ),
            new TestCaseData(
                null,
                "4.0",
                default(int)
            ),
            new TestCaseData(
                null,
                "test string",
                "default"
            ),
            new TestCaseData(
                "yyyy/dd/MM",
                "\"2020-31-10\"",
                default(DateTime)
            ),
            new TestCaseData(
                "yyyy/MM/dd",
                "\"2020-31-10\"",
                default(DateTime)
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                "\"2020-10-31T26:55:20+00:00\"",
                default(DateTime)
            ),
            new TestCaseData(
                null,
                "\"some string\"",
                new List<string>()
            ),
        };
    }
}
