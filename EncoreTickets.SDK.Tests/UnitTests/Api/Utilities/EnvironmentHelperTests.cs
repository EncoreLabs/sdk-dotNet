using System;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Api.Utilities
{
    internal class EnvironmentHelperTests
    {
        [TestCase("dev", Environments.Sandbox)]
        [TestCase("qa", Environments.QA)]
        [TestCase("staging", Environments.Staging)]
        [TestCase("prod", Environments.Production)]
        [TestCase("production", Environments.Production)]
        [TestCase("pRoD", Environments.Production)]
        [TestCase("DEV", Environments.Sandbox)]
        public void GetApiEnvironmentByName_IfValidNameIsProvided_ReturnsCorrectEnvironment(string name, Environments expectedResult)
        {
            var environment = EnvironmentHelper.GetApiEnvironmentByName(name);

            Assert.AreEqual(expectedResult, environment);
        }

        [TestCase("invalid")]
        [TestCase("stage")]
        [TestCase("sandbox")]
        [TestCase("Sandbox")]
        public void GetApiEnvironmentByName_IfInvalidNameIsProvided_ThrowsException(string name)
        {
            Assert.Throws<ArgumentException>(() => EnvironmentHelper.GetApiEnvironmentByName(name));
        }
    }
}
