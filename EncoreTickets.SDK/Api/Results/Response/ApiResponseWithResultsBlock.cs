namespace EncoreTickets.SDK.Api.Results.Response
{
    internal class ApiResponseWithResultsBlock<T> : BaseWrappedApiResponse<ApiResponseWithResultsBlockContent<T>, T>
        where T : class
    {
        /// <inheritdoc/>
        public override T Data => Response.Results ?? Response.Result;
    }
}