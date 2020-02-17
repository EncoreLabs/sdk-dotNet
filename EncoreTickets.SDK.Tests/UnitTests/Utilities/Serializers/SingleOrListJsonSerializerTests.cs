using System;
using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Tests.Helpers;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Venue.Models;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Serializers
{
    internal class SingleOrListJsonSerializerTests
    {
        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.Serialize_ReturnsCorrectJson))]
        public void Serialize_ReturnsCorrectJson<T>(string dateFormat, T item, string expected)
        {
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            var actual = serializer.Serialize(item);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserializedAndShouldNotBeWrappedAsList_ReturnsCorrectObject))]
        public void DeserializeRestResponse_IfJsonCanBeDeserializedAndShouldNotBeWrappedAsList_ReturnsCorrectObject<T>(string dateFormat, string json, T expected)
        {
            var response = new RestResponse<T> { Content = json };
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            var actual = serializer.Deserialize<T>(response);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserializedAndShouldBeWrappedAsList_ReturnsListWithCorrectObject))]
        public void DeserializeRestResponse_IfJsonCanBeDeserializedAndShouldBeWrappedAsList_ReturnsListWithCorrectObject<T>(string dateFormat, string json, List<T> expected)
        {
            var response = new RestResponse<List<T>> { Content = json };
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            var actual = serializer.Deserialize<List<T>>(response);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException))]
        public void DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException<T>(string dateFormat, string json, T defaultWithDeserializedType)
        {
            var response = new RestResponse<T> { Content = json };
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            Assert.Catch(() =>
            {
                var actual = serializer.Deserialize<T>(response);
            });
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserializedAndShouldNotBeWrappedAsList_ReturnsCorrectObject))]
        public void DeserializeString_IfJsonCanBeDeserializedAndShouldNotBeWrappedAsList_ReturnsCorrectObject<T>(string dateFormat, string json, T expected)
        {
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            var actual = serializer.Deserialize<T>(json);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.Deserialize_IfJsonCanBeDeserializedAndShouldBeWrappedAsList_ReturnsListWithCorrectObject))]
        public void DeserializeString_IfJsonCanBeDeserializedAndShouldBeWrappedAsList_ReturnsListWithCorrectObject<T>(string dateFormat, string json, List<T> expected)
        {
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            var actual = serializer.Deserialize<List<T>>(json);

            AssertExtension.AreObjectsValuesEqual(expected, actual);
        }

        [TestCaseSource(typeof(SingleOrListJsonSerializerTestsSource), nameof(SingleOrListJsonSerializerTestsSource.DeserializeRestResponse_IfJsonCannotBeDeserialized_ThrowsException))]
        public void DeserializeString_IfJsonCannotBeDeserialized_ThrowsException<T>(string dateFormat, string json, T defaultWithDeserializedType)
        {
            var serializer = new SingleOrListJsonSerializer<T> { DateFormat = dateFormat };

            Assert.Catch(() =>
            {
                var actual = serializer.Deserialize<T>(json);
            });
        }
    }

    public static class SingleOrListJsonSerializerTestsSource
    {
        public static IEnumerable<TestCaseData> Serialize_ReturnsCorrectJson = new[]
        {
            new TestCaseData(
                null,
                1,
                "1"
            ),
            new TestCaseData(
                null,
                -18901,
                "-18901"
            ),
            new TestCaseData(
                null,
                4.1234567890,
                "4.123456789"
            ),
            new TestCaseData(
                null,
                4M,
                "4.0"
            ),
            new TestCaseData(
                null,
                "test string",
                "\"test string\""
            ),
            new TestCaseData(
                null,
                true,
                "true"
            ),
            new TestCaseData(
                null,
                new object(),
                "{}"
            ),
            new TestCaseData(
                null,
                new List<object>(),
                "[]"
            ),
            new TestCaseData(
                null,
                new List<string>(),
                "[]"
            ),
            new TestCaseData(
                null,
                new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9},
                "[1,2,3,4,5,6,7,8,9]"
            ),
            new TestCaseData(
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
                new
                {
                    DOUBLE = true
                },
                "{\"double\":true}"
            ),
            new TestCaseData(
                null,
                new
                {
                    dEcImAl = 0.2456M
                },
                "{\"dEcImAl\":0.2456}"
            ),
            new TestCaseData(
                null,
                new DateTime(2020, 12, 31, 23, 59, 59),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                null,
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                null,
                new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Unspecified),
                "\"2020-12-31T23:59:59Z\""
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                new DateTime(2020, 10, 31, 16, 55, 20, DateTimeKind.Utc),
                "\"2020-10-31T16:55:20+00:00\""
            ),
            new TestCaseData(
                "yyyy-MM-dd",
                new DateTime(2020, 10, 31, 23, 59, 59, DateTimeKind.Utc),
                "\"2020-10-31\""
            ),
            new TestCaseData(
                null,
                Environments.Production,
                "\"production\""
            ),
            new TestCaseData(
                null,
                (Environments) 250,
                "250"
            ),
            new TestCaseData(
                null,
                new
                {
                    Enum = Environments.QA
                },
                "{\"enum\":\"qa\"}"
            ),
            new TestCaseData(
                null,
                new
                {
                    Intention = Intention.Positive
                },
                "{\"intention\":\"positive\"}"
            ),
        };

        public static IEnumerable<TestCaseData> Deserialize_IfJsonCanBeDeserializedAndShouldNotBeWrappedAsList_ReturnsCorrectObject = new[]
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

        public static IEnumerable<TestCaseData> Deserialize_IfJsonCanBeDeserializedAndShouldBeWrappedAsList_ReturnsListWithCorrectObject = new[]
        {
            new TestCaseData(
                null,
                "1",
                new List<int>{1}
            ),
            new TestCaseData(
                null,
                "-18901",
                new List<int>{-18901}
            ),
            new TestCaseData(
                null,
                "4.123456789",
                new List<double>{4.1234567890}
            ),
            new TestCaseData(
                null,
                "4.0",
                new List<decimal>{4M}
            ),
            new TestCaseData(
                null,
                "4.0",
                new List<double>{4D}
            ),
            new TestCaseData(
                null,
                "\"test string\"",
                new List<string>{"test string"}
            ),
            new TestCaseData(
                null,
                "true",
                new List<bool>{true}
            ),
            new TestCaseData(
                null,
                "\"false\"",
                new List<bool>{false}
            ),
            new TestCaseData(
                null,
                "{}",
                new List<object>{new object()}
            ),
            new TestCaseData(
                null,
                "[]",
                new List<List<object>>()),
            new TestCaseData(
                null,
                "\"2020-12-31T23:59:59Z\"",
                new List<DateTime> {new DateTime(2020, 12, 31, 23, 59, 59, DateTimeKind.Utc)}
            ),
            new TestCaseData(
                "yyyy-MM-ddTHH:mm:sszzz",
                "\"2020-10-31T16:55:20+00:00\"",
                new List<DateTime> {new DateTime(2020, 10, 31, 16, 55, 20, DateTimeKind.Utc)}
            ),
            new TestCaseData(
                "dd/MM/yyyy",
                "\"2020-10-31\"",
                new List<DateTime> {new DateTime(2020, 10, 31, 0,0,0, DateTimeKind.Utc)}
            ),
            new TestCaseData(
                null,
                "\"production\"",
                new List<Environments> {Environments.Production}
            ),
            new TestCaseData(
                null,
                "250",
                new List<Environments> {(Environments) 250}
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
                "[1,2,3]",
                new List<List<string>>{new List<string>{"1", "2", "3"}}
            ),
            new TestCaseData(
                null,
                "{\"intention\":\"positive\"}",
                new List<object> {new{Intention = Intention.Positive}}
            ),
            new TestCaseData(
                null,
                "{\"intention\":1000}",
                new List<object> {new{Intention = (Intention)1000}}
            ),
            new TestCaseData(
                null,
                "[1,2,3,4,5,6,7,8,9]",
                new List<List<int>>{new List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9}}
            ),
            new TestCaseData(
                null,
                "{\"number\":100,\"doubl\":9.087,\"str\":\"test string\",\"double\":true,\"dEcImAl\":0.2456}",
                new List<object> {
                    new
                    {
                        number = 100,
                        doubl = 9.087,
                        str = "test string",
                        DOUBLE = true,
                        dEcImAl = 0.2456M
                    }
                }
            ),
            new TestCaseData(
                null,
                "{\"someList\":[{\"someObject\":{},\"number\":100},{\"someString\":\"string\"}]}",
                new List<object> {
                    new
                    {
                        SomeList = new List<object>
                        {
                            new object(),
                            new object()
                        }
                    }
                }
            ),
        };
    }
}
