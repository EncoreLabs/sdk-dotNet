using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The base api class to be extended by concrete implementations
    /// </summary>
    public abstract class BaseApi
    {
        /// <summary>
        /// The host
        /// </summary>
        private readonly string host;

        /// <summary>
        /// The api context
        /// </summary>
        protected ApiContext Context;

        protected string BaseUrl => "https://" + string.Format(host, Context.Environment);

        protected virtual ApiRequestExecutor Executor => new ApiRequestExecutor(Context, BaseUrl);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        protected BaseApi(ApiContext context, string host)
        {
            this.host = host;
            Context = context;
        }
    }
}
