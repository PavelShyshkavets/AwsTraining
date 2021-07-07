using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Interfaces
{
    public interface IRepository
    {
        Task<Book> Single(Guid Id);

        Task<BookModel> All(string paginationToken = "");

        Task<IEnumerable<Book>> Find(SearchRequest searchReq);

        Task Add(BookInputModel entity);

        Task Remove(Guid Id);

        Task Update(Guid Id, BookInputModel entity);
    }
}
