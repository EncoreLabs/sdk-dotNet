using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Mapping;
using NUnit.Framework;
using RestSharp;
using DataFormat = EncoreTickets.SDK.Utilities.Enums.DataFormat;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities.Mapping
{
    internal class CommonProfileMappingTests
    {
        [TestCase(RequestMethod.Get, Method.GET)]
        [TestCase(RequestMethod.Post, Method.POST)]
        [TestCase(RequestMethod.Put, Method.PUT)]
        [TestCase(RequestMethod.Patch, Method.PATCH)]
        [TestCase(RequestMethod.Delete, Method.DELETE)]
        public void FromRequestMethodToMethod_IfRequestMethodExists_MapsCorrectly(RequestMethod source, Method expected)
        {
            var result = source.Map<RequestMethod, Method>();

            Assert.AreEqual(expected, result);
        }

        [TestCase(1111)]
        public void FromRequestMethodToMethod_IfRequestMethodDoesNotExist_ThrowsException(RequestMethod source)
        {
            Assert.Catch(() => source.Map<RequestMethod, Method>());
        }

        [TestCase(DataFormat.Xml, RestSharp.DataFormat.Xml)]
        [TestCase(DataFormat.Json, RestSharp.DataFormat.Json)]
        public void FromDataFormatToRestSharpDataFormat_IfDataFormatExists_MapsCorrectly(
            DataFormat source,
            RestSharp.DataFormat expected)
        {
            var result = source.Map<DataFormat, RestSharp.DataFormat>();

            Assert.AreEqual(expected, result);
        }

        [TestCase(1111)]
        public void FromDataFormatToRestSharpDataFormat_IfDataFormatDoesNotExist_ThrowsException(DataFormat source)
        {
            Assert.Catch(() => source.Map<DataFormat, RestSharp.DataFormat>());
        }
    }
}