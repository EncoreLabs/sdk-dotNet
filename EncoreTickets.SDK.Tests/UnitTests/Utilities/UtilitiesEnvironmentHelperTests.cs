using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Utilities;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.UnitTests.Utilities
{
    internal class UtilitiesEnvironmentHelperTests
    {
        [TestCase("dev", Environments.Sandbox)]
        [TestCase("qa", Environments.QA)]
        [TestCase("staging", Environments.Staging)]
        [TestCase("prod", Environments.Production)]
        [TestCase("production", Environments.Production)]
        [TestCase("pRoD", Environments.Production)]
        [TestCase("DEV", Environments.Sandbox)]
        public void Utilities_EnvironmentsExtension_EnvironmentFromName_IfValidNameIsProvided_ReturnsCorrectEnvironment(string name, Environments expectedResult)
        {
            var environment = EnvironmentExtension.EnvironmentFromName(name);

            Assert.AreEqual(expectedResult, environment);
        }

        [Test]
        public void Utilities_EnvironmentsExtension_EnvironmentFromName_IfInvalidNameIsProvided_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => EnvironmentExtension.EnvironmentFromName("invalid"));
        }
    }
}
