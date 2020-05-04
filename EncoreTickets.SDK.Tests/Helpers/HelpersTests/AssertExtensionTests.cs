using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Helpers.HelpersTests
{
    [TestFixture]
    internal class AssertExtensionTests
    {
        [TestCaseSource(typeof(SourcesForAssertExtensionTests), nameof(SourcesForAssertExtensionTests.AreObjectsValuesEqual_AssertsCorrectly))]
        public void AreObjectsValuesEqual_AssertsCorrectly<T>(T expected, T actual)
        {
            //Act + Assert
            Assert.DoesNotThrow(() => AssertExtension.AreObjectsValuesEqual(expected, actual));
        }

        [TestCaseSource(typeof(SourcesForAssertExtensionTests), nameof(SourcesForAssertExtensionTests.AreObjectsValuesEqual_ThrowsAssertionException))]
        public void AreObjectsValuesEqual_ThrowsAssertionException<T>(T expected, T actual)
        {
            //Act + Assert
            Assert.Throws<AssertionException>(() => AssertExtension.AreObjectsValuesEqual(expected, actual));
        }
    }

    internal static class SourcesForAssertExtensionTests
    {
        public static TestCaseData[] AreObjectsValuesEqual_AssertsCorrectly => new[]
        {
            new TestCaseData("some string", "some string") {TestName = "Same strings"},
            new TestCaseData(100, 100) {TestName = "Same integers"},
            new TestCaseData(-45.109, -45.109) {TestName = "Same doubles"},
            new TestCaseData(true, true) {TestName = "Same booleans"},
            new TestCaseData(
                    new DateTime(2019, 11, 13, 19, 34, 45),
                    new DateTime(2019, 11, 13, 19, 34, 45))
                {TestName = "Same DateTime objects"},
            new TestCaseData(
                    new List<object> {true, 11.3, -12345, "string"},
                    new List<object> {true, 11.3, -12345, "string"})
                {TestName = "Same lists with objects of basic types"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, 11.3, -12345, "string"})
                {TestName = "Same object arrays from objects of basic types"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"},
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"})
                {TestName = "Same objects with properties of basic types"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes(),
                    new ObjectWithPropertiesOfBasicTypes())
                {TestName = "Same objects with uninitialized properties of basic types"},
            new TestCaseData(
                    new ObjectWithPropertyOfGenericType<List<string>>(),
                    new ObjectWithPropertyOfGenericType<List<string>>())
                {TestName = "Same generic objects"},
            new TestCaseData(
                    new ObjectWithPropertyOfGenericType<List<string>> {Object = new List<string> {"TEST"}},
                    new ObjectWithPropertyOfGenericType<List<string>> {Object = new List<string> {"TEST"}})
                {TestName = "Same objects with IEnumerable properties"},
            new TestCaseData(
                    new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                        {Object = new ObjectWithPropertyOfGenericType<string> {Object = "TEST"}},
                    new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                        {Object = new ObjectWithPropertyOfGenericType<string> {Object = "TEST"}})
                {TestName = "Same objects with properties of custom types"},
            new TestCaseData(
                    new object[] {new ObjectWithPropertyOfGenericType<object>()},
                    new object[] {new ObjectWithPropertyOfGenericType<object>()})
                {TestName = "Same object arrays from empty objects of custom types"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    })
                {TestName = "Same object arrays from objects with property of a basic type"},
            new TestCaseData(
                    new object[]
                    {
                        new object[]
                        {
                            new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                            new ObjectWithPropertyOfGenericType<int> {Object = 345},
                            new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                            new ObjectWithPropertyOfGenericType<bool> {Object = false}
                        }
                    },
                    new object[]
                    {
                        new object[]
                        {
                            new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                            new ObjectWithPropertyOfGenericType<int> {Object = 345},
                            new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                            new ObjectWithPropertyOfGenericType<bool> {Object = false}
                        }
                    })
                {TestName = "Same object arrays from arrays of objects with property of a basic type"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                            {Object = new ObjectWithPropertyOfGenericType<string> {Object = "TEST"}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                            {Object = new ObjectWithPropertyOfGenericType<int> {Object = 345}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<double>>
                            {Object = new ObjectWithPropertyOfGenericType<double> {Object = -45.78}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<bool>>
                            {Object = new ObjectWithPropertyOfGenericType<bool> {Object = false}}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                            {Object = new ObjectWithPropertyOfGenericType<string> {Object = "TEST"}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                            {Object = new ObjectWithPropertyOfGenericType<int> {Object = 345}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<double>>
                            {Object = new ObjectWithPropertyOfGenericType<double> {Object = -45.78}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<bool>>
                            {Object = new ObjectWithPropertyOfGenericType<bool> {Object = false}}
                    })
                {TestName = "Same object arrays from objects with property of a custom type"},
        };

        public static object[] AreObjectsValuesEqual_ThrowsAssertionException => new object[]
        {
            new TestCaseData("some string", "another string") {TestName = "Different strings"},
            new TestCaseData(100, -100) {TestName = "Different integers"},
            new TestCaseData(-45.109, -45.1099) {TestName = "Different doubles"},
            new TestCaseData(true, false) {TestName = "Different booleans"},
            new TestCaseData(
                    new DateTime(2019, 11, 13, 19, 34, 45),
                    new DateTime(2019, 11, 13, 19, 34, 46))
                {TestName = "Different DateTime objects"},
            new TestCaseData(
                    new List<object> {true, 11.3, -12345, "string"},
                    new List<object> {true, -12345, 11.3, "string"})
                {TestName = "Same lists with objects of basic types in a different order"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, -12345, "string", 11.3})
                {TestName = "Same object arrays from objects of basic types in a different order"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, 11.3, -12345})
                {TestName = "Arrays from same objects but one missing"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {false, 11.3, -12345, "string"})
                {TestName = "Arrays from same objects except a boolean object"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, 0.3, -12345, "string"})
                {TestName = "Arrays from same objects except a double object"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, 11.3, 12345, "string"})
                {TestName = "Arrays from same objects except an int object"},
            new TestCaseData(
                    new object[] {true, 11.3, -12345, "string"},
                    new object[] {true, 11.3, -12345, "another string"})
                {TestName = "Arrays from same objects except a string object"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"},
                    new ObjectWithPropertiesOfBasicTypes {Bool = false, Double = 11.3, Int = -12345, String = "string"})
                {TestName = "Objects with same properties of basic types except a boolean property"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"},
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 81.3, Int = -12345, String = "string"})
                {TestName = "Objects with same properties of basic types except a double property"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"},
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -145, String = "string"})
                {TestName = "Objects with same properties of basic types except an int property"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "string"},
                    new ObjectWithPropertiesOfBasicTypes {Bool = true, Double = 11.3, Int = -12345, String = "another string"})
                {TestName = "Objects with same properties of basic types except a string property"},
            new TestCaseData(
                    new ObjectWithPropertiesOfBasicTypes(),
                    null)
                {TestName = "Objects with uninitialized properties and null"},
            new TestCaseData(
                    new ObjectWithPropertyOfGenericType<List<string>> {Object = new List<string> {"TEST"}},
                    new ObjectWithPropertyOfGenericType<List<string>> {Object = new List<string> {"another string"}})
                {TestName = "Objects with different IEnumerable properties"},
            new TestCaseData(
                    new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                        {Object = new ObjectWithPropertyOfGenericType<int> {Object = 0}},
                    new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                        {Object = new ObjectWithPropertyOfGenericType<int> {Object = 1}})
                {TestName = "Objects with different properties of same custom types"},
            new TestCaseData(
                    new object[] {new ObjectWithPropertyOfGenericType<object>()},
                    new object[] {new object()})
                {TestName = "Object arrays from objects of different reference types"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = ""},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    })
                {TestName = "Object arrays from same objects with property of a basic type except an object with String property"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 3},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    })
                {TestName = "Object arrays from same objects with property of a basic type except an object with Int property"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -0.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    })
                {TestName = "Object arrays from same objects with property of a basic type except an object with Double property"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = false}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                        new ObjectWithPropertyOfGenericType<int> {Object = 345},
                        new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                        new ObjectWithPropertyOfGenericType<bool> {Object = true}
                    })
                {TestName = "Object arrays from same objects with property of a basic type except an object with Bool property"},
            new TestCaseData(
                    new object[]
                    {
                        new object[]
                        {
                            new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                            new ObjectWithPropertyOfGenericType<int> {Object = 345},
                            new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                            new ObjectWithPropertyOfGenericType<bool> {Object = false}
                        }
                    },
                    new object[]
                    {
                        new object[]
                        {
                            new ObjectWithPropertyOfGenericType<string> {Object = "TEST"},
                            new ObjectWithPropertyOfGenericType<int> {Object = 3},
                            new ObjectWithPropertyOfGenericType<double> {Object = -45.78},
                            new ObjectWithPropertyOfGenericType<bool> {Object = false}
                        }
                    })
                {TestName = "Object arrays from arrays of objects with same property of a basic type except one object"},
            new TestCaseData(
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                            {Object = new ObjectWithPropertyOfGenericType<string> {Object = "TEST"}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                            {Object = new ObjectWithPropertyOfGenericType<int> {Object = 345}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<double>>
                            {Object = new ObjectWithPropertyOfGenericType<double> {Object = -45.78}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<bool>>
                            {Object = new ObjectWithPropertyOfGenericType<bool> {Object = false}}
                    },
                    new object[]
                    {
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<string>>
                            {Object = new ObjectWithPropertyOfGenericType<string> {Object = ""}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<int>>
                            {Object = new ObjectWithPropertyOfGenericType<int> {Object = 345}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<double>>
                            {Object = new ObjectWithPropertyOfGenericType<double> {Object = -45.78}},
                        new ObjectWithPropertyOfGenericType<ObjectWithPropertyOfGenericType<bool>>
                            {Object = new ObjectWithPropertyOfGenericType<bool> {Object = false}}
                    })
                {TestName = "Object arrays from objects with same property of a custom type except one object"},
        };
    }
}