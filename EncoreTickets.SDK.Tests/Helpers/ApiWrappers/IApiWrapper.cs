using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal interface IApiWrapper
    {
        ApiContext SourceContext { get; }

        string SourceHost { get; }
    }
}