using System;
using System.Threading.Tasks;
using Contract.Interfaces;
using Contract.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ISqsService _sqsService;
        private readonly ILogger _logger;

        public BookController(ILogger<BookController> logger, IRepository repository, ISqsService sqsService)
        {
            _repository = repository;
            _sqsService = sqsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            _logger.LogInformation("Get all");
            var readers = await _repository.GetAll();
            return Ok(readers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var readers = await _repository.GetById(id);
                _logger.LogInformation("Get By id: " + JsonConvert.SerializeObject(readers));
                return Ok(readers);
            }
            catch
            {
                _logger.LogInformation("Get By id exeption!");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Book model)
        {
            try
            {
                await _sqsService.SendMessage(model, MessageType.Create);
                await _repository.Create(model);
                _logger.LogInformation("Create: " + JsonConvert.SerializeObject(model));
                return Ok();
            }
            catch
            {
                _logger.LogInformation("Create exeption!");
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Book model)
        {
            try
            {
                await _sqsService.SendMessage(model, MessageType.Update);
                await _repository.Update(model);
                _logger.LogInformation("Update: " + JsonConvert.SerializeObject(model));
                return Ok();
            }
            catch
            {
                _logger.LogInformation("Update exeption!");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.Delete(id);
                _logger.LogInformation("Delete: " + id);
                return Ok();
            }
            catch (Exception)
            {
                _logger.LogInformation("Delete exeption!");
                return BadRequest();
            }
        }
    }
}
