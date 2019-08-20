using System;
using EncoreTickets.SDK.Api.Context;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiErrorEventArgTests
    {
        [Test]
        public void EntertainApi_ApiErrorEventArg_Constructor_InitializesProperties()
        {
            var exception = new Exception();
            const string message = "test";
            var args = new ApiErrorEventArgs(exception, message);
            Assert.AreEqual(message, args.Message);
            Assert.AreEqual(exception, args.Exception);
        }
    }
}
