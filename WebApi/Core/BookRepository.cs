using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Contract;
using Contract.Enums;
using Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BookRepository : IRepository
    {
        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        public BookRepository()
        {
            _client = new AmazonDynamoDBClient("AKIAQNO7NYHQI4N6VTV3", "zTWT1ZwzoqPFMpi7rGmGrLAcfgCL7suAMtgy+zAj", Amazon.RegionEndpoint.USWest2);
            _context = new DynamoDBContext(_client);
        }

        public async Task Add(BookInputModel entity)
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

        public async Task<BookModel> All(string paginationToken = "")
        {
            var table = _context.GetTargetTable<Book>();

            var scanOps = new ScanOperationConfig();

            if (!string.IsNullOrEmpty(paginationToken))
            {
                scanOps.PaginationToken = paginationToken;
            }

            var results = table.Scan(scanOps);
            List<Document> data = await results.GetNextSetAsync();

            IEnumerable<Book> books = _context.FromDocuments<Book>(data);

            return new BookModel
            {
                PaginationToken = results.PaginationToken,
                Books = books,
                ResultsType = ResultsType.List
            };

        }

        public async Task<IEnumerable<Book>> Find(SearchRequest searchReq)
        {
            var scanConditions = new List<ScanCondition>();
            if (!string.IsNullOrEmpty(searchReq.ISBN))
                scanConditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, searchReq.ISBN));
            if (!string.IsNullOrEmpty(searchReq.Description))
                scanConditions.Add(new ScanCondition("Description", ScanOperator.Equal, searchReq.Description));
            if (!string.IsNullOrEmpty(searchReq.Name))
                scanConditions.Add(new ScanCondition("Name", ScanOperator.Equal, searchReq.Name));

            return await _context.ScanAsync<Book>(scanConditions, null).GetRemainingAsync();
        }

        public async Task Remove(Guid id)
        {
            await _context.DeleteAsync<Book>(id);
        }

        public async Task<Book> Single(Guid id)
        {
            return await _context.LoadAsync<Book>(id);
        }

        public async Task Update(Guid id, BookInputModel entity)
        {
            var reader = await Single(id);

            reader.ISBN = entity.ISBN;
            reader.Description = entity.Description;
            reader.Name = entity.Name;

            await _context.SaveAsync(reader);
        }
    }
}
