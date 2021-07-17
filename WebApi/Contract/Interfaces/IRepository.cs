using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.Model;

namespace Contract.Interfaces
{
    public interface IRepository
    {
        Task<Book> GetById(Guid Id);

        Task<IEnumerable<Book>> GetAll();

        Task Create(Book entity);

        Task Delete(Guid Id);

        Task Update(Book entity);
    }
}
