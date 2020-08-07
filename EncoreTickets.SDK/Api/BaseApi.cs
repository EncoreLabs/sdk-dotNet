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

        /// <summary>
        /// Gets the version of API used by the service.
        /// </summary>
        public int? ApiVersion => LegacyModeEnabled ? LegacyApiVersion : LatestApiVersion;

        /// <summary>
        /// Gets a value indicating whether legacy mode is enabled.
        /// </summary>
        public bool LegacyModeEnabled { get; }

        /// <inheritdoc />
        public ApiContext Context { get; set; }

        /// <summary>
        /// Gets the version of API to use without legacy mode.
        /// </summary>
        protected abstract int? LatestApiVersion { get; }

        /// <summary>
        /// Gets the version of API to use in legacy mode if such mode is supported by the service.
        /// </summary>
        protected virtual int? LegacyApiVersion => LatestApiVersion;

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
        /// Initialises a new instance of the <see cref="BaseApi"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="host">The host.</param>
        /// <param name="useLegacyMode">Indicates whether the service should be used in legacy mode.</param>
        protected BaseApi(ApiContext context, string host, bool useLegacyMode = true)
        {
            Host = host;
            Context = context;
            LegacyModeEnabled = useLegacyMode;
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
