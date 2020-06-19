using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace EncoreTickets.SDK.Aws
{
    /// <summary>
    /// The interface to work with AWS Simple Queue Service.
    /// </summary>
    public interface IAwsSqs
    {
        /// <summary>
        /// Asynchronously sends a message to the specified AWS queue.
        /// </summary>
        /// <param name="queueUrl">The URL of the AWS queue.</param>
        /// <param name="messageBody">The message to send to the queue.</param>
        /// <returns>The SendMessageResponse object containing the response from AWS.</returns>
        Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody);
    }
}
