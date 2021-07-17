using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Contract.Interfaces;
using Contract.Model;

namespace Core
{
    internal class SqsService : ISqsService
    {
        private readonly AmazonSQSClient _client;

        public SqsService()
        {
            _client = new AmazonSQSClient(RegionEndpoint.USWest2);
        }

        public async Task SendMessage(Book book, MessageType messageType)
        {
            var message = new SqsMessage
            {
                Book = book,
                MessageType = messageType,
            };

            var request = new GetQueueUrlRequest
            {
                QueueName = "BookQueue",
                QueueOwnerAWSAccountId = "028922724832"
            };

            var response = await _client.GetQueueUrlAsync(request);
            var sendMessageRequest = new SendMessageRequest(response.QueueUrl, JsonConvert.SerializeObject(message));
            await _client.SendMessageAsync(sendMessageRequest);
        }
    }
}
