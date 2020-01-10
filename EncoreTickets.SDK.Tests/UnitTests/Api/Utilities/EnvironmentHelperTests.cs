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
        public void EnvironmentFromName_IfValidNameIsProvided_ReturnsCorrectEnvironment(string name, Environments expectedResult)
        {
            var environment = EnvironmentHelper.EnvironmentFromName(name);

            Assert.AreEqual(expectedResult, environment);
        }

        [Test]
        public void EnvironmentFromName_IfInvalidNameIsProvided_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => EnvironmentHelper.EnvironmentFromName("invalid"));
        }
    }
}
