using EncoreTickets.SDK.Api.Models;

namespace EncoreTickets.SDK.Tests.Helpers.ApiWrappers
{
    internal interface IApiWrapper
    {
        ApiContext SourceContext { get; }

        string SourceHost { get; }
    }
}