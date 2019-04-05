using System;
using System.Collections.Generic;
using System.Text;

namespace EncoreTickets.SDK
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
