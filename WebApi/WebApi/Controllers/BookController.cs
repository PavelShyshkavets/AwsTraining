using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Contract.Interfaces;
using Contract;
using Contract.Enums;

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
            var readers = await _repository.All();
            return Ok(readers);
        }

        [HttpGet("{iSBN}")]
        public async Task<ActionResult> GetById(string iSBN)
        {
            if (!string.IsNullOrEmpty(iSBN))
            {
                var readers = await _repository.Find(new SearchRequest { ISBN = iSBN });
                return Ok(readers.FirstOrDefault());
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] BookInputModel model)
        {
            try
            { 
                await _repository.Add(model);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Edit(Guid id, [FromBody] BookInputModel model)
        {
            try
            {

                await _repository.Update(id, model);
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
                await _repository.Remove(id);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
