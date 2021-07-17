using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Contract.Interfaces;
using Contract.Model;

namespace Core
{
    internal class BookRepository : IRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public BookRepository()
        {
            _client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.USWest2);
            _context = new DynamoDBContext(_client);
        }

        public async Task Create(Book entity)
        {
            var reader = new Book
            {
                Id = Guid.NewGuid(),
                Name = entity.Name,
                Description = entity.Description,
                ISBN = entity.ISBN,
            };

            await _context.SaveAsync(reader);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var results = _context.GetTargetTable<Book>().Scan(new ScanOperationConfig());
            var data = await results.GetNextSetAsync();

            return _context.FromDocuments<Book>(data);

        }

        public async Task Delete(Guid id)
        {
            await _context.DeleteAsync<Book>(id);
        }

        public async Task<Book> GetById(Guid id)
        {
            return await _context.LoadAsync<Book>(id);
        }

        public async Task Update(Book entity)
        {
            var reader = await GetById(entity.Id);

            reader.ISBN = entity.ISBN;
            reader.Description = entity.Description;
            reader.Name = entity.Name;

            await _context.SaveAsync(reader);
        }
    }
}
