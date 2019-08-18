using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Api
{
    public abstract class BaseCapabilityServiceApi : BaseApi
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        protected BaseCapabilityServiceApi(ApiContext context, string host) : base(context, host)
        {
        }
    }
}
