using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Api.Utilities.RestClientBuilder;

namespace EncoreTickets.SDK.Api
{
    /// <summary>
    /// The base api class to be extended by concrete implementations.
    /// </summary>
    public abstract class BaseApi : IServiceApi
    {
        private readonly IApiRestClientBuilder restClientBuilder;

        public abstract int? ApiVersion { get; }

        /// <inheritdoc />
        public ApiContext Context { get; set; }

        /// <summary>
        /// Gets base API URL.
        /// </summary>
        protected virtual string BaseUrl => "https://" + string.Format(Host, GetEnvironmentPartOfHost());

        /// <summary>
        /// Gets an executor of requests to the service based on context and base URL.
        /// </summary>
        protected virtual ApiRequestExecutor Executor => new ApiRequestExecutor(Context, BaseUrl, restClientBuilder);

        /// <summary>
        /// Gets API host.
        /// </summary>
        protected string Host { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        protected BaseApi(ApiContext context, string host)
        {
            Host = host;
            Context = context;
            restClientBuilder = new ApiRestClientBuilder();
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
                default:
                    return "dev";
            }
        }
    }
}
