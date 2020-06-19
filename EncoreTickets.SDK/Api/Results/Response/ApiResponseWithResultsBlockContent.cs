namespace EncoreTickets.SDK.Api.Results.Response
{
    internal class ApiResponseWithResultsBlockContent<T>
        where T : class
    {
        public T Results { get; set; }

        public T Result { get; set; }
    }
}