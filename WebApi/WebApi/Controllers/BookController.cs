using System;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Contract.Interfaces;
using Contract.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Amazon;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository _repository;

        public BookController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var readers = await _repository.GetAll();
            return Ok(readers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var readers = await _repository.GetById(id);
                return Ok(readers);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Book model)
        {
            try
            {
                var client = new AmazonSQSClient("AKIAQNO7NYHQL3ZQMREH", "KgoP33i/XmE0dTq3QH9KwgYPrbMxn2AyrgUAkDnt", RegionEndpoint.USWest2);
                var request = new GetQueueUrlRequest
                {
                    QueueName = "BookQueue",
                    QueueOwnerAWSAccountId = "028922724832"
                };

                var response = await client.GetQueueUrlAsync(request);
                var sendMessageRequest = new SendMessageRequest(response.QueueUrl, JsonSerializer.Serialize(model));
                var sendMessageResponse = await client.SendMessageAsync(sendMessageRequest);

                await _repository.Create(model);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Book model)
        {
            try
            {

                await _repository.Update(model);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
