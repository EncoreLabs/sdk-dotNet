using EncoreTickets.SDK.Api.Context;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The base api class to be extended by concrete implementations.
    /// </summary>
    public abstract class BaseApi
    {
        private readonly string host;

        /// <summary>
        /// Gets base API URL.
        /// </summary>
        protected virtual string BaseUrl => "https://" + string.Format(host, GetEnvironmentPartOfHost());

        /// <summary>
        /// Gets an executor of requests to the service based on context and base URL.
        /// </summary>
        protected virtual ApiRequestExecutor Executor => new ApiRequestExecutor(Context, BaseUrl);

        /// <summary>
        /// Gets or sets API context.
        /// </summary>
        protected ApiContext Context { get; set; }

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

        private string GetEnvironmentPartOfHost()
        {
            switch (Context.Environment)
            {
                case Environments.Production:
                    return "";
                case Environments.Staging:
                    return "staging";
                case Environments.QA:
                    return "qa";
                case Environments.Sandbox:
                default:
                    return "dev";
            }
        }
    }
}
