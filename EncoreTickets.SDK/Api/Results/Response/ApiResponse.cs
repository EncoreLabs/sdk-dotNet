namespace EncoreTickets.SDK.Api.Results.Response
{
    public class ApiResponse<T> : BaseWrappedApiResponse<T, T>
        where T : class
    {
        public override T Data => response;
    }
}