using System;
using EncoreTickets.SDK.Api.Context;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiContextTests
    {
        private static readonly object[] SourceForConstructorTest =
        {
            new ApiContext(), 
            new ApiContext(Environments.Sandbox), 
            new ApiContext(Environments.Sandbox, "test", "test"), 
        };

        private static readonly object[] SourceForOnErrorOccurredTest =
        {
            new object[]
            {
                new EventHandler<ApiErrorEventArgs>((sender, args) => { }),
                true
            },
            new object[]
            {
                null,
                false
            },
        };

        [TestCaseSource(nameof(SourceForConstructorTest))]
        public void ApiContext_Constructor_InitializesEnvironment(ApiContext context)
        {
            Assert.IsNotNull(context.Environment);
        }

        [TestCaseSource(nameof(SourceForOnErrorOccurredTest))]
        public void ApiContext_OnErrorOccurred_ReturnsCorrectly(EventHandler<ApiErrorEventArgs> apiError, bool expected)
        {
            ApiContext.ApiError += apiError;

            var result = ApiContext.OnErrorOccurred(It.IsAny<object>(), It.IsAny<ApiErrorEventArgs>());
            Assert.AreEqual(expected, result);

            ApiContext.ApiError -= apiError;
        }
    }
}
