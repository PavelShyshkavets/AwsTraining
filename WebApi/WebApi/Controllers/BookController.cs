using System;
using System.Threading.Tasks;
using Contract.Interfaces;
using Contract.Model;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ISqsService _sqsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public BookController(IRepository repository, ISqsService sqsService)
        {
            _repository = repository;
            _sqsService = sqsService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            logger.Info("Get all");
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
                await _sqsService.SendMessage(model, MessageType.Create);
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
                await _sqsService.SendMessage(model, MessageType.Update);
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
