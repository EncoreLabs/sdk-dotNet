using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace EncoreTickets.SDK.Aws
{
    public interface IAwsSqs
    {
        Task<SendMessageResponse> SendMessageAsync(string queueUrl, string messageBody);
    }
}
